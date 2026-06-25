using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Commands
{
    public record VerifyMobileNumberOtpCommand(string OtpCode): IRequest<ResultResponse<string>>;


}
