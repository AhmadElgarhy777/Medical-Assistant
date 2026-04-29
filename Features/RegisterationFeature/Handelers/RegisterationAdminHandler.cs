using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.RegisterationFeature.Commands;
using Features.RegisterationFeature.Events.Event;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models;
using Services.EmailServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace Features.RegisterationFeature.Handelers
{
    internal class RegisterationAdminHandler : IRequestHandler<RegisterationAdminCommand, ResultResponse<String>>
    {
        private readonly IMediator mediator;
        private readonly IEmailServices emailServices;
        private readonly IUnitOfWork unitOfWork;
        private readonly IAdminRepositry adminRepositry;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RegisterationAdminHandler(
            IMediator mediator,
            IEmailServices emailServices,
            IUnitOfWork unitOfWork,
            IAdminRepositry adminRepositry,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.mediator = mediator;
            this.emailServices = emailServices;
            this.unitOfWork = unitOfWork;
            this.adminRepositry = adminRepositry;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<ResultResponse<String>> Handle(RegisterationAdminCommand request, CancellationToken cancellationToken)
        {
            // ── 1. Seed roles if not exist ──────────────────────────────────────
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new(SD.SuperAdminRole));
                await roleManager.CreateAsync(new(SD.AdminRole));
                await roleManager.CreateAsync(new(SD.PatientRole));
                await roleManager.CreateAsync(new(SD.NurseRole));
                await roleManager.CreateAsync(new(SD.DoctorRole));
                await roleManager.CreateAsync(new(SD.PharmacyRole));
            }

            var AdminDto = request.Admin;

            // ── 2. Check if email already exists ───────────────────────────────
            var IsExist = await userManager.FindByEmailAsync(AdminDto.Email);
            if (IsExist != null)
            {
                return new ResultResponse<String>
                {
                    ISucsses = false,
                    Message = "This email is already registered."
                };
            }

            // ── 3. Generate a secure random temp password (NOT SSN-based) ──────
            var tempPassword = GenerateSecureTempPassword();

            // ── 4. Build Identity user and Admin entity ─────────────────────────
            var AdminUser = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = AdminDto.UserName,
                Email = AdminDto.Email,
                Address = AdminDto.Address,
                Gender = AdminDto.Gender,
                Role = SD.AdminRole,
                City = AdminDto.City,
                Governorate = AdminDto.Governorate,
                
            };

            var admin = new Admin
            {
                ID = AdminUser.Id,
                SSN=AdminDto.SSN,
                UserName = AdminDto.UserName,
                Email = AdminDto.Email,
                Address = AdminDto.Address,
                Gender = AdminDto.Gender,
                City = AdminDto.City,
                Governorate = AdminDto.Governorate,
            };

            // ── 5. Transaction ──────────────────────────────────────────────────
            var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Create user with temp password
                var createResult = await userManager.CreateAsync(AdminUser, tempPassword);
                if (!createResult.Succeeded)
                {
                    return new ResultResponse<String>
                    {
                        ISucsses = false,
                        Message = "Failed to create user.",
                        Errors = createResult.Errors.Select(e => e.Description).ToList()
                    };
                }

                // Assign Admin role
                await userManager.AddToRoleAsync(AdminUser, SD.AdminRole);

                // Save Admin record
                adminRepositry.Add(admin);
                await adminRepositry.CommitAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                // ── 6. Generate tokens AFTER successful save ────────────────────

                // Token to confirm email
                var emailConfirmToken = await userManager.GenerateEmailConfirmationTokenAsync(AdminUser);

                // Token to let admin set their own password
                var setPasswordToken = await userManager.GeneratePasswordResetTokenAsync(AdminUser);

                // ── 7. Publish invitation event (sends the email) ───────────────
                await mediator.Publish(new AdminInvitationEvent(
                    AdminUser.Id,
                    AdminUser.Email,
                    AdminUser.UserName,
                    emailConfirmToken,
                    setPasswordToken,
                    cancellationToken));

                return new ResultResponse<String>()
                {
                    ISucsses = true,
                    Message = "Admin account created. An invitation email has been sent."
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Registration failed. No data was saved.",
                    Errors = new List<string> { ex.Message ,ex.InnerException.ToString() }
                };
            }
        }

        // ── Helper: secure random password ─────────────────────────────────────
        private static string GenerateSecureTempPassword()
        {
            // Satisfies Identity rules: upper, lower, digit, special
            // Admin will never use this — they set their own via the email link
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                          .Replace("=", "")
                          .Replace("+", "")
                          .Replace("/", "") + "Aa1!";
        }
    }
}
