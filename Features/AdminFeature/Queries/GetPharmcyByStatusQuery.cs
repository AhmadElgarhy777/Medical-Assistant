using MediatR;
using Models.DTOs;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AdminFeature.Queries
{
    public record GetPharmcyByStatusQuery(CancellationToken CancellationToken, ConfrmationStatus status, int page = 1) : IRequest<ResultResponse<List<PharmacyRowDTO>>>;

}
