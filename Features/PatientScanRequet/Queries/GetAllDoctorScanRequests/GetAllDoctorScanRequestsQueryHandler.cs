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

namespace Features.PatientScanRequet.Queries.GetAllDoctorScanRequests
{
    public class GetAllDoctorScanRequestsQueryHandler
       : IRequestHandler<GetAllDoctorScanRequestsQuery,
           ResultResponse<List<AllDoctorScanRequestsQueryDto>>>
    {
        private readonly IScanRequestRepository repository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetAllDoctorScanRequestsQueryHandler(
            IScanRequestRepository repository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResultResponse<List<AllDoctorScanRequestsQueryDto>>> Handle(
            GetAllDoctorScanRequestsQuery request,
            CancellationToken cancellationToken)
        {
            var doctorId = httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            if (doctorId is null)
            {
                return new ResultResponse<List<AllDoctorScanRequestsQueryDto>>
                {
                    ISucsses = false,
                    Message = "Unauthorized."
                };
            }

            var query = repository
                .GetTable()
                .AsNoTracking()
                .Include(x => x.Patient)
                .Include(x => x.Images)
                .Include(x => x.AiReport)
                .Where(x => x.DoctorId == doctorId);

            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }

            var result = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new AllDoctorScanRequestsQueryDto
                {
                    ScanRequestId = x.ID,

                    PatientId = x.PatientId,

                    PatientName = x.Patient.UserName,

                    AIModelType = x.AIModelType,

                    Status = x.Status,

                    ExpirationDate = x.ExpirationDate,

                    ImagesCount = x.Images.Count,

                    HasReport = x.AiReport != null
                })
                .ToListAsync(cancellationToken);

            return new ResultResponse<List<AllDoctorScanRequestsQueryDto>>
            {
                ISucsses = true,
                Obj = result
            };
        }
    }
}
