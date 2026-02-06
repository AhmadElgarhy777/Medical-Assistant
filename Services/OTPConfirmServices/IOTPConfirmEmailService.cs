using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OTPConfirmServices
{
    public interface IOTPConfirmEmailService
    {
        Task SendAsync(string UserId, string Email, CancellationToken cancellationToken=default);
        Task<bool> VerifyAsync(string UserId, string Otp, CancellationToken cancellationToken=default);
    }
}
