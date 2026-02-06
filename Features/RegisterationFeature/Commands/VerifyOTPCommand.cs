using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Commands
{
    public record VerifyOTPCommand(string UserId, string Otp, CancellationToken CancellationToken) : IRequest<ResultResponse<String>>;
}
