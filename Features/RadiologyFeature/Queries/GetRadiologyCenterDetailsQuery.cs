using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Models.DTOs;

namespace Features.RadiologyFeature.Queries
{
    public record GetRadiologyCenterDetailsQuery(string CenterId, string? Search) : IRequest<LabDetailsDTO?>;
}