using MediatR;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AdminFeature.Queries
{
    public record GetDoctorDetailsQuery(string doctorId,CancellationToken cancellationToken):IRequest<ResultResponse<DoctorDetailsDTO>>;
}
