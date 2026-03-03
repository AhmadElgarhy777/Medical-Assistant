using DataAccess.Repositry.IRepositry;
using Features.RegisterationFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTOs.RegistertionDTOs;
using Services.ImageServices;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace Features.RegisterationFeature.Handelers
{
    public class RegistrationPharmacyHandler : IRequestHandler<RegistrationPharmacyCommand, ResultResponse<String>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IImageService imageService;
        private readonly IConfiguration configuration;
        private readonly IPharmacyRepository pharmacyRepository;

        public RegistrationPharmacyHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IImageService imageService,
            IConfiguration configuration,
            IPharmacyRepository pharmacyRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.imageService = imageService;
            this.configuration = configuration;
            this.pharmacyRepository = pharmacyRepository;
        }

        public async Task<ResultResponse<string>> Handle(RegistrationPharmacyCommand request, CancellationToken cancellationToken)
        {
            var dto = request.PharmacyDTO;

            // تأكد إن الإيميل مش موجود
            var checkUser = await userManager.FindByEmailAsync(dto.Email);
            if (checkUser != null)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "الإيميل ده موجود بالفعل!"
                };
            }

            // عمل الـ ApplicationUser
            var appUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                City = dto.City,
                Governorate = dto.Governorate,
                Gender = dto.Gender,
                Role = SD.PharmacyRole
            };

            // رفع الصورة لو موجودة
            if (dto.Img is not null)
            {
                var image = await imageService.UploadImgAsync(dto.Img, "PharmacyImages", cancellationToken);
                appUser.Img = $"{configuration["ApiBaseUrl"]}/PharmacyImages/{image}";
            }

            // عمل الـ User
            var result = await userManager.CreateAsync(appUser, dto.Password);
            if (!result.Succeeded)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "مش قدر يعمل الحساب!",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            // ✅ تأكد إن الـ Role موجود
            if (!await roleManager.RoleExistsAsync(SD.PharmacyRole))
                await roleManager.CreateAsync(new IdentityRole(SD.PharmacyRole));

            // إضافة الـ Role
            await userManager.AddToRoleAsync(appUser, SD.PharmacyRole);

            // عمل الـ Pharmacy
            var pharmacy = new Pharmacy
            {
                ID = appUser.Id,
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.PhoneNumber,
                City = dto.City,
                Governorate = dto.Governorate.ToString(),
                Gender = dto.Gender.ToString(),
                PharmacyLicense = dto.PharmacyLicense,
                Status = "Active",
                BD = dto.BirthDate,
                RealImg = appUser.Img
            };

            // ✅ حفظ الصيدلية في الـ Database
            await pharmacyRepository.AddPharmacyAsync(pharmacy);

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "تم تسجيل الصيدلية بنجاح!",
                Data = appUser.Id
            };
        }
    }
}