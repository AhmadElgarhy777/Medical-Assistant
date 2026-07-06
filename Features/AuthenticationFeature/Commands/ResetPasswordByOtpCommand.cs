using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Commands
{
    public record ResetPasswordByOtpCommand(
        ResetPasswordByOtpDto ResetDto)
        : IRequest<ResultResponse<string>>;


    public class ResetPasswordByOtpDto
    {
        public string Email { get; set; } = null!;

        public string Otp { get; set; } = null!;

        public string NewPassword { get; set; } = null!;
    }
}
