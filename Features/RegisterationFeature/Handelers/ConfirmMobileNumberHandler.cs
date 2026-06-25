using Features.RegisterationFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Services.TwilioProviderServices.WhatsUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Handelers
{
    public class ConfirmMobileNumberHandler : IRequestHandler<ConfirmMobileNumberCommand, ResultResponse<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISmsService smsService;
        private readonly IMemoryCache cache;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ConfirmMobileNumberHandler(
            UserManager<ApplicationUser> userManager,
            ISmsService smsService,
            IMemoryCache cache,
            IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.smsService = smsService;
            this.cache = cache;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResultResponse<string>> Handle(ConfirmMobileNumberCommand request, CancellationToken cancellationToken)
        {
            // ── 1. Get logged-in user from JWT ───────────────────────────────
            var userId = httpContextAccessor.HttpContext?.User
                            .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Fail("Unauthorized.");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Fail("User not found.");

            // ── 2. Generate 6-digit OTP ──────────────────────────────────────
            var otp = new Random().Next(100000, 999999).ToString();

            // ── 3. Store OTP in cache — expires in 5 minutes ─────────────────
            var cacheKey = $"OTP_{userId}";
            cache.Set(cacheKey, otp, TimeSpan.FromMinutes(5));

            // ── 4. Save phone number on user (unverified for now) ────────────
            var normalizedPhone = NormalizeEgyptianPhone(request.PhoneNumber);
            user.PhoneNumber = normalizedPhone;
            await userManager.UpdateAsync(user);

            // ── 5. Send OTP via WhatsApp ─────────────────────────────────────

            await smsService.SendOtpAsync(
                normalizedPhone,
                $"Your verification code is: *{otp}*\nValid for 5 minutes. Do not share it with anyone.");

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "OTP sent via WhatsApp successfully."
            };
        }
        private static ResultResponse<string> Fail(string msg) =>
            new() { ISucsses = false, Message = msg };
        private static string NormalizeEgyptianPhone(string phone)
        {
            phone = phone.Trim().Replace(" ", "").Replace("-", "");

            // If starts with 0 → replace with +20
            if (phone.StartsWith("0"))
                return "+20" + phone.Substring(1);

            // If starts with 20 (without +) → add +
            if (phone.StartsWith("20"))
                return "+" + phone;

            // If already correct format
            if (phone.StartsWith("+20"))
                return phone;

            return phone;
        }
    }
   
 }
   



  

