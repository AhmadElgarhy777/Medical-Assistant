using Features.RegisterationFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Handelers
{
    internal class VerifyMobileNumberOtpHandler : IRequestHandler<VerifyMobileNumberOtpCommand, ResultResponse<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMemoryCache cache;
        private readonly IHttpContextAccessor httpContextAccessor;

        public VerifyMobileNumberOtpHandler(
            UserManager<ApplicationUser> userManager,
            IMemoryCache cache,
            IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.cache = cache;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultResponse<string>> Handle(VerifyMobileNumberOtpCommand request, CancellationToken cancellationToken)
        {
            // ── 1. Get logged-in user from JWT ───────────────────────────────
            var userId = httpContextAccessor.HttpContext?.User
                            .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Fail("Unauthorized.");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Fail("User not found.");

            // ── 2. Get OTP from cache ────────────────────────────────────────
            var cacheKey = $"OTP_{userId}";
            var cachedOtp = cache.Get<string>(cacheKey);

            if (cachedOtp == null)
                return Fail("OTP has expired. Please request a new one.");

            // ── 3. Compare OTP ───────────────────────────────────────────────
            if (cachedOtp != request.OtpCode)
                return Fail("Invalid OTP code.");

            // ── 4. Mark phone as verified in database ────────────────────────
            user.PhoneNumberConfirmed = true;
            await userManager.UpdateAsync(user);

            // ── 5. Remove OTP from cache — one time use only ─────────────────
            cache.Remove(cacheKey);

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "Phone number verified successfully ✅"
            };
        }
        private static ResultResponse<string> Fail(string msg) =>
           new() { ISucsses = false, Message = msg };
    }
}
