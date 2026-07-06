using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class RegisterLabHandler : IRequestHandler<RegisterLabCommand, ResultResponse<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;
        private readonly ILabRepository _labRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterLabHandler(
            UserManager<ApplicationUser> userManager,
            IImageService imageService,
            IConfiguration configuration,
            ILabRepository labRepository,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _imageService = imageService;
            _configuration = configuration;
            _labRepository = labRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultResponse<string>> Handle(RegisterLabCommand request, CancellationToken cancellationToken)
        {
            var dto = request.LabDTO;

            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            {
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "الإيميل ده موجود بالفعل!"
                };
            }

            string? imgUrl = null;
            if (dto.Img is not null)
            {
                var image = await _imageService.UploadImgAsync(dto.Img, "LabImages", cancellationToken);
                imgUrl = $"{_configuration["ApiBaseUrl"]}/LabImages/{image}";
            }

            var userId = Guid.NewGuid().ToString();

            var appUser = new ApplicationUser
            {
                Id = userId,
                UserName = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                City = dto.Address, // مؤقتًا بناخد نفس العنوان لحد ما نضيف حقل City منفصل
                Role = SD.LabRole,
                Img = imgUrl
            };

            var lab = new Lab
            {
                ID = userId,
                Name = dto.Name,
                Email = dto.Email,
                Address = dto.Address,
                Phone = dto.PhoneNumber,
                AreaId = dto.AreaId,
                LabLicense = dto.LabLicense,
                Status = ConfrmationStatus.Pending,
                ImageUrl = imgUrl,
                SupportsHomeCollection = dto.SupportsHomeCollection,
                IsActive = false, // لسه مش متفعل لحد ما الأدمن يوافق
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                IsDeleted = false
            };

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

                await _userManager.AddToRoleAsync(appUser, SD.LabRole);

                await _labRepository.AddLabAsync(lab);

                await _unitOfWork.CompleteAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new ResultResponse<string>
                {
                    ISucsses = true,
                    Message = "تم إرسال طلب تسجيل المعمل، هيتفعل بعد موافقة الأدمن",
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
                        ex.InnerException?.Message ?? "no inner exception"
                    }
                };
            }
        }
    }
}