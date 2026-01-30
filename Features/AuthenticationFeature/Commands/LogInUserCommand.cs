using MediatR;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Quieries
{
    public record LogInUserCommand(UserLoginDTO User ,CancellationToken CancellationToken):IRequest<AuthDTO>;
    
}
