using Features.RegisterationFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Features.RegisterationFeature.Handelers
{
    internal class RegisterationAdminHandler : IRequestHandler<RegisterationAdminCommand, ResultResponse<String>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RegisterationAdminHandler(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<ResultResponse<String>> Handle(RegisterationAdminCommand request, CancellationToken cancellationToken)
        {
            if(roleManager.Roles==null|| roleManager.Roles?.Count() == 0)
            {
                await roleManager.CreateAsync(new(SD.AdminRole));
                await roleManager.CreateAsync(new(SD.PatientRole));
                await roleManager.CreateAsync(new(SD.NurseRole));
                await roleManager.CreateAsync(new(SD.DoctorRole));
            }

            var AdminDto = request.Admin;
            var pass = $"Admin@{AdminDto.SSN}";
            //sending an email to the admin that contain the Pass and User Name and the data that enter 

            var IsExist=await userManager.FindByEmailAsync(AdminDto.Email);
            if (IsExist != null)
            {
               return new ResultResponse<String>{
                    ISucsses = false,
                   Message = "The User Is Already Exist"
                };
            }

            var AdminUser = new ApplicationUser()
            {
                Id=Guid.NewGuid().ToString(),
                UserName = $"{AdminDto.FName.ToUpper()}{AdminDto.LName.ToUpper()}",
                Email = AdminDto.Email,
                Address=AdminDto.Address,
                Gender=AdminDto.Gender,
                Role=SD.AdminRole,
                City=AdminDto.City,
                Governorate=AdminDto.Governorate,
            };
            if (AdminUser != null)
            {

               
                    var user = await userManager.CreateAsync(AdminUser, pass);
                    if (user.Succeeded)
                    {
                        await userManager.AddToRoleAsync(AdminUser, SD.AdminRole);
                        return new ResultResponse<String>()
                        {
                            ISucsses = true,
                            Message = "The User Is Succesfully Created "
                            
                        };
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
            return new ResultResponse<String>
            {
                ISucsses = false,
             Message = "The User does not Created , Please contact with Admin"
            };
        }
    }
}
