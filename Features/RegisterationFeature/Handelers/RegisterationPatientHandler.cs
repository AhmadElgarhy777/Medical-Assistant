using AutoMapper;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.RegisterationFeature.Commands;
using Services.ImageServices;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTOs.RegistertionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Features.RegisterationFeature.Handelers
{
    public class RegisterationPatientHandler : IRequestHandler<RegisterationPatientCommand, ResultResponse<String>>
    {
        private readonly IPatientRepositry patientRepositry;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        private readonly IImageService imageService;
        private readonly IConfiguration configuration;

        public RegisterationPatientHandler(
            IPatientRepositry patientRepositry,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IImageService imageService,
            IConfiguration configuration
            )
        {
            this.patientRepositry = patientRepositry;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.imageService = imageService;
            this.configuration = configuration;
        }
        public async Task<ResultResponse<string>> Handle(RegisterationPatientCommand request, CancellationToken cancellationToken)
        {
            var patientDto = request.PatientDTO;
            var Role = SD.PatientRole;
            var checkuser = await userManager.FindByEmailAsync(patientDto.Email);
            if (checkuser != null)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "The User Is Already Exist",
                };
            }


            var appuser = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = patientDto.UserName,
                Email = patientDto.Email,
                Address = patientDto.AddressInDetails,
                Gender = patientDto.Gender,
                Role = Role,
            };

            if (appuser is not null)
            {
                var user = await userManager.CreateAsync(appuser, patientDto.Password);
                if (user.Succeeded)
                {
                    await userManager.AddToRoleAsync(appuser, Role);

                    var patient = mapper.Map<RegisterPatientDTO, Patient>(patientDto);
                    patient.ID = appuser.Id;

                    if (patientDto.Img is not null)
                    {
                        var image = await imageService.UploadImgAsync(patientDto.Img, "PatientImages", cancellationToken);
                        patient.Img = $"{configuration["ApiBaseUrl"]}/{image}";
                    }

                    if (patient is not null)
                    {
                        

                        await patientRepositry.AddAsync(patient);
                        await patientRepositry.CommitAsync(cancellationToken);
                        return new ResultResponse<string>
                        {
                            ISucsses = true,
                            Message = "The User Is Added Succesfully"
                        };
                    }
                }
                else
                {
                    return new ResultResponse<String>
                    {
                        ISucsses = false,
                        Message = "The User does not Created ",
                        Errors = user.Errors.Select(e => e.Description).ToList()
                    };
                }

            }
            return new ResultResponse<string>
            {
                ISucsses = false,
                Message = "The User Is Not Added"
            };

        }
    }
}
