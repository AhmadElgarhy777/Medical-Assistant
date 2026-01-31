using AutoMapper;
using Features.RegisterationFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models.DTOs.RegistertionDTOs;
using Models.DTOs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using DataAccess.Repositry.IRepositry;
using Services.ImageServices;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Configuration;

namespace Features.RegisterationFeature.Handelers
{
    public class RegisterationNurseHandler : IRequestHandler<RegisterationNurseCommand, ResultResponse<String>>
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        private readonly INuresRepositry nuresRepositry;
        private readonly IImageService imageService;
        private readonly IConfiguration configuration;

        public RegisterationNurseHandler(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            INuresRepositry nuresRepositry,
            IImageService imageService,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.nuresRepositry = nuresRepositry;
            this.imageService = imageService;
            this.configuration = configuration;
        }
        public async Task<ResultResponse<string>> Handle(RegisterationNurseCommand request, CancellationToken cancellationToken)
        {
            var nurseDto = request.NurseDTO;
            var Role = SD.NurseRole;
            var checkuser = await userManager.FindByEmailAsync(nurseDto.Email);
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
                UserName = nurseDto.UserName,
                Email = nurseDto.Email,
                Address = nurseDto.AddressInDetails,
                Gender = nurseDto.Gender,
                Role = Role,
            };

            if (appuser is not null)
            {
                var user = await userManager.CreateAsync(appuser, nurseDto.Password);
                if (user.Succeeded)
                {
                    await userManager.AddToRoleAsync(appuser, Role);
                    var nurse = mapper.Map<RegisterNurseDTO, Nures>(nurseDto);
                    nurse.ID = appuser.Id;

                    if(nurseDto.Img is not null&& nurseDto.CertificationImg is not null&& nurseDto.CrediateImg is not null)
                    {
                        var profileImg = await imageService.UploadImgAsync(nurseDto.Img, "NurseImages/ProfileImages", cancellationToken);
                        var CertifcationImg = await imageService.UploadImgAsync(nurseDto.CertificationImg, "NurseImages/CertificationImages", cancellationToken);
                        var CrediateImg = await imageService.UploadImgAsync(nurseDto.CrediateImg, "NurseImages/CrediateImages", cancellationToken);

                        nurse.Img= $"{configuration["ApiBaseUrl"]}/NurseImages/ProfileImages/{profileImg}";
                        nurse.CertificationImg= $"{configuration["ApiBaseUrl"]}/NurseImages/CertificationImages/{CertifcationImg}";
                        nurse.CrediateImg= $"{configuration["ApiBaseUrl"]}/NurseImages/CrediateImages/{CrediateImg}";
                    }

                    if (nurse is not null)
                    {
                        await nuresRepositry.AddAsync(nurse);
                        await nuresRepositry.CommitAsync(cancellationToken);
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
