using MediatR;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Commands
{
    public record RegisterationAdminCommand(AdminUserDTO Admin):IRequest<ResultResponse<String>>;
    
}
