using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.RegisterationFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTOs.RegistertionDTOs;
using Models.Enums;
using Services.ImageServices;
using Utility;

namespace Features.RegisterationFeature.Handelers
{
    public class RegistrationPharmacyHandler : IRequestHandler<RegistrationPharmacyCommand, ResultResponse<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;
        private readonly IPharmacyRepository _pharmacyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegistrationPharmacyHandler(
            UserManager<ApplicationUser> userManager,
            IImageService imageService,
            IConfiguration configuration,
            IPharmacyRepository pharmacyRepository,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _imageService = imageService;
            _configuration = configuration;
            _pharmacyRepository = pharmacyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultResponse<string>> Handle(RegistrationPharmacyCommand request, CancellationToken cancellationToken)
        {
            var dto = request.PharmacyDTO;

            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "الإيميل ده موجود بالفعل!"
                };
            }

            // 2. رفع الصورة قبل الـ Transaction عشان متفتحش connection وانت بترفع
            string? imgUrl = null;
            if (dto.Img is not null)
            {
                var image = await _imageService.UploadImgAsync(dto.Img, "PharmacyImages", cancellationToken);
                imgUrl = $"{_configuration["ApiBaseUrl"]}/PharmacyImages/{image}";
            }

            // 3. بناء الـ Entities
            var userId = Guid.NewGuid().ToString();

            var appUser = new ApplicationUser
            {
                Id = userId,
                UserName = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                City = dto.City,
                Governorate = dto.Governorate,
                Gender = dto.Gender,
                Role = SD.PharmacyRole,
                Img = imgUrl
            };

            var pharmacy = new Pharmacy
            {
                ID = userId,
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.PhoneNumber,
                //Email = dto.Email,
                City = dto.City,
                Governorate = dto.Governorate.ToString(),
                Gender = dto.Gender.ToString(),
                PharmacyLicense = dto.PharmacyLicense,
                Status = ConfrmationStatus.Pending,
                BD = dto.BirthDate,
                RealImg = imgUrl,
                Latitude = dto.Latitude,   // ✅
                Longitude = dto.Longitude  // ✅
            };

            // 4. حفظ الاتنين في Transaction واحدة
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var createResult = await _userManager.CreateAsync(appUser, dto.Password);
                if (!createResult.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new ResultResponse<string>
                    {
                        ISucsses = false,
                        Message = "مش قدر يعمل الحساب!",
                        Errors = createResult.Errors.Select(e => e.Description).ToList()
                    };
                }

                await _userManager.AddToRoleAsync(appUser, SD.PharmacyRole);

                await _pharmacyRepository.AddPharmacyAsync(pharmacy);

                await _unitOfWork.CompleteAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new ResultResponse<string>
                {
                    ISucsses = true,
                    Message = "تم تسجيل الصيدلية بنجاح!",
                    Data = userId
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "فشل التسجيل، مفيش حاجة اتحفظت.",
                    Errors = new List<string> {
                        
                        ex.Message,
                        ex.InnerException?.Message ?? "no inner exception",
                        ex.StackTrace ?? "no stack trace"

                    }
                };
            }
        }
    }
}