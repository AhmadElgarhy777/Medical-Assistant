using MediatR;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Queries
{
    public record GetAllSpecializationQuery(CancellationToken CancellationToken):IRequest<ResultResponse<List<SpecializationDTO>>>;
   
}
