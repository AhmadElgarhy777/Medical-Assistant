using MediatR;
using Models.DTOs.RegistertionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Commands
{
    public record RegisterationNurseCommand(RegisterNurseDTO NurseDTO,CancellationToken CancellationToken):IRequest<ResultResponse<String>>;
  
}
