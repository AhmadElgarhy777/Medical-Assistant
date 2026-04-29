using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.SuperAdminFeature.Command
{
    public record UpdateSuperAdminCommand(string CurrentPassword, string? NewEmail, string? NewUserName, string? NewPassword, string? ConfirmNewPassword): IRequest<ResultResponse<string>>;


}
