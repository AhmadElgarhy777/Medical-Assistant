using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Models.DTOs;
using Models.Enums;

namespace Features.LabFeature.Queries
{
    public record GetAreasByGovernorateQuery(Governorate Governorate) : IRequest<List<AreaDTO>>;
}