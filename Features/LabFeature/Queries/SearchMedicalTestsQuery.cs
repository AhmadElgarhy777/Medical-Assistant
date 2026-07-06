using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Models.DTOs;

namespace Features.LabFeature.Queries
{
    public record SearchMedicalTestsQuery(string? Search) : IRequest<List<MedicalTestCardDTO>>;
}