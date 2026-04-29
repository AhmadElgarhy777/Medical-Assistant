using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Commands
{
    public record AccepAdmintInvitationCommand( string UserId, string EmailToken, string PasswordToken, string NewPassword, string ConfirmPassword) :IRequest<ResultResponse<string>>;

}
