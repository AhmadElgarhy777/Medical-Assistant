using AutoMapper;
using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.RegisterationFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Models;
using Models.DTOs;
using Models.DTOs.RegistertionDTOs;
using Services.ImageServices;
using Utility;

namespace Features.RegisterationFeature.Handelers
{
    public class RegisterationDoctorHandler : IRequestHandler<RegisterationDoctorCommand, ResultResponse<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IDoctorRepositry _doctorRepositry;
        private readonly IConfiguration _configuration;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork unitOfWork;

        public RegisterationDoctorHandler(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IDoctorRepositry doctorRepositry,
            IConfiguration configuration,
            IImageService imageService,
            IUnitOfWork unitOfWork
            )
        {
            _userManager = userManager;
            _mapper = mapper;
            _doctorRepositry = doctorRepositry;
            _configuration = configuration;
            _imageService = imageService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ResultResponse<string>> Handle(RegisterationDoctorCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Doctor;

            // 1. Check for duplicate email upfront
            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            {
                return Fail("A user with this email already exists.");
            }

            // 2. Upload images before starting the transaction (avoids holding a DB tx while doing I/O)
            string? profileImgUrl = null, certImgUrl = null, credImgUrl = null;

            if (dto.Img is not null && dto.CertificationImg is not null && dto.CrediateImg is not null)
            {
                var baseUrl = _configuration["ApiBaseUrl"];

                var profileFile = await _imageService.UploadImgAsync(dto.Img, "DoctorImages/ProfileImages", cancellationToken);
                var certFile = await _imageService.UploadImgAsync(dto.CertificationImg, "DoctorImages/CertificationImages", cancellationToken);
                var credFile = await _imageService.UploadImgAsync(dto.CrediateImg, "DoctorImages/CrediateImages", cancellationToken);

                profileImgUrl = $"{baseUrl}/DoctorImages/ProfileImages/{profileFile}";
                certImgUrl = $"{baseUrl}/DoctorImages/CertificationImages/{certFile}";
                credImgUrl = $"{baseUrl}/DoctorImages/CrediateImages/{credFile}";
            }

            // 3. Build entities
            var userId = Guid.NewGuid().ToString();

            var appUser = new ApplicationUser
            {
                Id = userId,
                UserName = dto.UserName,
                Email = dto.Email,
                Address = dto.AddressInDetails,
                Gender = dto.Gender,
                Role = SD.DoctorRole,
                PhoneNumber = dto.Phone,
                City = dto.City,
                Governorate = dto.Governorate,
                Img = profileImgUrl
            };

            var doctor = _mapper.Map<RegisterDoctorDTO, Doctor>(dto);
            doctor.ID = userId;
            doctor.Img = profileImgUrl;
            doctor.CertificationImg = certImgUrl;
            doctor.CrediateImg = credImgUrl;

            // 4. Persist both inside a single transaction
            await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var createResult = await _userManager.CreateAsync(appUser, dto.Password);
                if (!createResult.Succeeded)
                {
                    return new ResultResponse<string>
                    {
                        ISucsses = false,
                        Message = "Failed to create user account.",
                        Errors = createResult.Errors.Select(e => e.Description).ToList()
                    };
                }

                await _userManager.AddToRoleAsync(appUser, SD.DoctorRole);

                _doctorRepositry.Add(doctor);
                await _doctorRepositry.CommitAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new ResultResponse<string>
                {
                    ISucsses = true,
                    Message = "Doctor registered successfully.",
                    Data = userId
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Registration failed. No data was saved.",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Small helper to keep return sites readable
        private static ResultResponse<string> Fail(string message) =>
            new() { ISucsses = false, Message = message };
    }
}