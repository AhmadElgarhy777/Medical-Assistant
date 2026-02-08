using AutoMapper;
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using Features.AuthenticationFeature.Commands;
using Features.AuthenticationFeature.Extentions.Updateing;
using Features.RegisterationFeature.Events.Event;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTOs;
using Services.ImageServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utility;
using static System.Net.Mime.MediaTypeNames;

namespace Features.AuthenticationFeature.Handlers
{
    public class UpdateProfileHabdler : IRequestHandler<UpdateProfileCommand, ResultResponse<UpdateProfileDTO>>
    {
        private readonly IHttpContextAccessor http;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPatientRepositry patientRepositry;
        private readonly IDoctorRepositry doctorRepositry;
        private readonly INuresRepositry nuresRepositry;
        private readonly IMediator mediator;
        private readonly IImageService imageService;
        private readonly IConfiguration configration;
        private readonly IMapper mapper;

        public UpdateProfileHabdler(IHttpContextAccessor http,
            UserManager<ApplicationUser> userManager,
            IPatientRepositry patientRepositry,
            IDoctorRepositry doctorRepositry,
            INuresRepositry nuresRepositry,
            IMediator mediator,
            IImageService imageService,
            IConfiguration configration,
            IMapper mapper)
        {
            this.http = http;
            this.userManager = userManager;
            this.patientRepositry = patientRepositry;
            this.doctorRepositry = doctorRepositry;
            this.nuresRepositry = nuresRepositry;
            this.mediator = mediator;
            this.imageService = imageService;
            this.configration = configration;
            this.mapper = mapper;
        }
        public async Task<ResultResponse<UpdateProfileDTO>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var userId=http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userUpdate = request.ProfileDTO;
            if(userId is null)
            {
                return new ResultResponse<UpdateProfileDTO>
                {
                    ISucsses = false,
                    Message = "The User Is Invaild"
                };
            }
            var user=await userManager.FindByIdAsync(userId);
            if(user is not null)
            {
                if (user.Role == SD.PatientRole)
                {
                    var updatePatient=new PatientProfileUpdate(patientRepositry,mediator,imageService,configration);
                    await updatePatient.UpdateAsync(user, userUpdate, cancellationToken);
                }
                else if (user.Role == SD.DoctorRole)
                {
                    var updateDoctor = new DoctorProfileUpdate(doctorRepositry, mediator, imageService, configration);
                    await updateDoctor.UpdateAsync(user, userUpdate, cancellationToken);
                }
                else if (user.Role == SD.NurseRole)
                {
                    var updateNurse = new NurseProfileUpdate(nuresRepositry, mediator, imageService, configration);
                    await updateNurse.UpdateAsync(user, userUpdate, cancellationToken);
                }

                await userManager.UpdateAsync(user);
                return new ResultResponse<UpdateProfileDTO>
                {
                    ISucsses = true,
                    Message = "The User Updated sucesfully .."
                };
            }


            return new ResultResponse<UpdateProfileDTO>
            {
                ISucsses = false,
                Message = "The User Not Found , Invaild"
            };

        }
    }
}
