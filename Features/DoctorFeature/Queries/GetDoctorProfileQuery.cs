using MediatR;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.DoctorFeature.Queries
{
    public record GetDoctorProfileQuery(string doctorId, CancellationToken CancellationToken) : IRequest<ResultResponse<DoctorDTO>>;

}
