using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Services.EmailServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.OTPConfirmServices
{
    public class OTPConfirmEmailService : IOTPConfirmEmailService
    {
        private readonly IMemoryCache cache;
        private readonly IEmailServices emailServices;
        private readonly UserManager<ApplicationUser> userManager;

        public OTPConfirmEmailService(IMemoryCache cache,IEmailServices emailServices,UserManager<ApplicationUser > userManager)
        {
            this.cache = cache;
            this.emailServices = emailServices;
            this.userManager = userManager;
        }
        public async Task SendAsync(string UserId, string Email, CancellationToken cancellationToken=default)
        {
            var otp = new Random().Next(100000, 999999).ToString();

            cache.Set($"otp_{UserId}", otp, TimeSpan.FromMinutes(5));
            await emailServices.SendEmailAsync(Email, "Confirmation OTP",
                $"Your Otp For Confirm Your Email IS \n ' {otp} '",
                cancellationToken
                );
        }

        public async Task<bool> VerifyAsync(string UserId, string Otp, CancellationToken cancellationToken = default)
        {
           var user =await userManager.FindByIdAsync(UserId);

            if (cache.TryGetValue($"otp_{UserId}",out string CachedOtp))
            {
                if (CachedOtp == Otp)
                {
                    cache.Remove($"otp_{UserId}");
                    return true;
                }
            }
            return false;
        }
    }
}
