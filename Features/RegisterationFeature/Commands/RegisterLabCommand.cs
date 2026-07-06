using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Models.DTOs.RegistertionDTOs;

namespace Features.RegisterationFeature.Commands
{
    public record RegisterLabCommand(RegisterLabDTO LabDTO, CancellationToken CancellationToken) : IRequest<ResultResponse<string>>;
}