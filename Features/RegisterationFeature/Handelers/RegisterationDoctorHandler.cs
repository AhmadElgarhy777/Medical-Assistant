using AutoMapper;
using DataAccess.Repositry.IRepositry;
using Features.RegisterationFeature.Commands;
using Services.ImageServices;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTOs;
using Models.DTOs.RegistertionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Features.RegisterationFeature.Handelers
{
    public class RegisterationDoctorHandler : IRequestHandler<RegisterationDoctorCommand, ResultResponse<String>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        private readonly IDoctorRepositry doctorRepositry;
        private readonly IConfiguration configuration;
        private readonly IImageService imageService;

        public RegisterationDoctorHandler(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IDoctorRepositry doctorRepositry,
            IConfiguration configuration,
            IImageService imageService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.doctorRepositry = doctorRepositry;
            this.configuration = configuration;
            this.imageService = imageService;
        }
        public async Task<ResultResponse<string>> Handle(RegisterationDoctorCommand request, CancellationToken cancellationToken)
        {
            var doctorDto = request.Doctor;
            var Role = SD.DoctorRole;
            var checkuser=await userManager.FindByEmailAsync(doctorDto.Email);
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
                Id=Guid.NewGuid().ToString(),
                UserName=doctorDto.UserName,
                Email=doctorDto.Email,
                Address=doctorDto.AddressInDetails,
                Gender=doctorDto.Gender,
                Role= Role,
            };

            if(appuser is not null)
            {
                var user = await userManager.CreateAsync(appuser, doctorDto.Password);
                if (user.Succeeded)
                {
                    await userManager.AddToRoleAsync(appuser, Role);
                    var doctor=mapper.Map<RegisterDoctorDTO, Doctor>(doctorDto);
                    doctor.ID = appuser.Id;

                    if (doctorDto.Img is not null && doctorDto.CertificationImg is not null && doctorDto.CrediateImg is not null)
                    {
                        var profileImg = await imageService.UploadImgAsync(doctorDto.Img, "DoctorImages/ProfileImages", cancellationToken);
                        var CertifcationImg = await imageService.UploadImgAsync(doctorDto.CertificationImg, "DoctorImages/CertificationImages", cancellationToken);
                        var CrediateImg = await imageService.UploadImgAsync(doctorDto.CrediateImg, "DoctorImages/CrediateImages", cancellationToken);

                        doctor.Img = $"{configuration["ApiBaseUrl"]}/{profileImg}";
                        doctor.CertificationImg = $"{configuration["ApiBaseUrl"]}/{CertifcationImg}";
                        doctor.CrediateImg = $"{configuration["ApiBaseUrl"]}/{CrediateImg}";
                    }

                    if (doctor is not null)
                    {
                        await doctorRepositry.AddAsync(doctor);
                        await doctorRepositry.CommitAsync(cancellationToken);
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
