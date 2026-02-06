using MediatR;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Commands
{
    public record ChangePasswordCommand(ChangePasswordDTO PasswordDTO,CancellationToken CancellationToken):IRequest<ResultResponse<String>>;
}
