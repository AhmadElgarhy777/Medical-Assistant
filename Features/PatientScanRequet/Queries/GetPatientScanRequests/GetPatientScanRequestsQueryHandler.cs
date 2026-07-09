using DataAccess.Repositry.IRepositry;
using Features.PatientScanRequet.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Queries.GetPatientScanRequests
{
    public class GetPatientScanRequestsQueryHandler
    : IRequestHandler<GetPatientScanRequestsQuery,
        ResultResponse<List<PatientScanRequestDto>>>
    {
        private readonly IScanRequestRepository repository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetPatientScanRequestsQueryHandler(
            IScanRequestRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultResponse<List<PatientScanRequestDto>>> Handle(
            GetPatientScanRequestsQuery request,
            CancellationToken cancellationToken)
        {
            var patientId = httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            if (patientId is null)
            {
                return new ResultResponse<List<PatientScanRequestDto>>
                {
                    ISucsses = false,
                    Message = "Unauthorized."
                };
            }

            var query = repository
                .GetTable()
                .AsNoTracking()
                .Include(x => x.Doctor)
                .Include(x => x.Images)
                .Where(x => x.PatientId == patientId);

            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }

            var result = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new PatientScanRequestDto
                {
                    ScanRequestId = x.ID,

                    DoctorId = x.DoctorId,

                    DoctorName = x.Doctor.UserName,

                    AIModelType = x.AIModelType,

                    Status = x.Status,

                    DoctorNote = x.DoctorNote,

                    RejectReason = x.RejectReason,

                    ExpirationDate = x.ExpirationDate,

                    ImagesCount = x.Images.Count,

                    HasUploadedImages = x.Images.Any()
                })
                .ToListAsync(cancellationToken);

            return new ResultResponse<List<PatientScanRequestDto>>
            {
                ISucsses = true,
                Obj = result
            };
        }
    }
}
