using AutoMapper;
using DataAccess;
using Features.RadiologyFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using System.Security.Claims;
using Models.Enums;

namespace Features.RadiologyFeature.Handlers
{
    public class PatientRadiologyQueryHandlers :
        IRequestHandler<GetRadiologyAreasQuery, ResultResponse<object>>,
        IRequestHandler<GetRadiologyByAreaQuery, ResultResponse<PaginatedList<object>>>,
        IRequestHandler<GetRadiologyDetailsForPatientQuery, ResultResponse<object>>,
        IRequestHandler<GetPatientRadiologyBookingsQuery, ResultResponse<PaginatedList<object>>>,
        IRequestHandler<GetPatientRadiologyBookingDetailsQuery, ResultResponse<object>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public PatientRadiologyQueryHandlers(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private string? GetPatientId() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<ResultResponse<object>> Handle(GetRadiologyAreasQuery request, CancellationToken cancellationToken)
        {
            var areas = await _context.Areas.Where(a => !a.IsDeleted).Select(a => new { a.ID, a.Name }).ToListAsync(cancellationToken);
            return new ResultResponse<object> { ISucsses = true, Obj = areas };
        }

        public async Task<ResultResponse<PaginatedList<object>>> Handle(GetRadiologyByAreaQuery request, CancellationToken cancellationToken)
        {
            var query = _context.RadiologyCenters
                .Where(l => l.AreaId == request.AreaId && l.Status == ConfrmationStatus.Approved && !l.IsDeleted && l.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Request.Search))
            {
                query = query.Where(l => l.Name.Contains(request.Request.Search));
            }

            var count = await query.CountAsync(cancellationToken);
            var centers = await query.Skip((request.Request.PageNumber - 1) * request.Request.PageSize)
                                  .Take(request.Request.PageSize)
                                  .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<RadiologyCenterDto>>(centers).Cast<object>().ToList();
            var paginated = new PaginatedList<object>(dtos, count, request.Request.PageNumber, request.Request.PageSize);

            return new ResultResponse<PaginatedList<object>> { ISucsses = true, Obj = paginated };
        }

        public async Task<ResultResponse<object>> Handle(GetRadiologyDetailsForPatientQuery request, CancellationToken cancellationToken)
        {
            var center = await _context.RadiologyCenters
                .Include(l => l.ScanOffers.Where(t => !t.IsDeleted)).ThenInclude(t => t.RadiologyScan)
                .FirstOrDefaultAsync(l => l.ID == request.Id && !l.IsDeleted && l.Status == ConfrmationStatus.Approved, cancellationToken);

            if (center == null) return new ResultResponse<object> { ISucsses = false, Message = "Radiology Center not found" };

            var centerDto = _mapper.Map<RadiologyCenterDto>(center);
            var scans = center.ScanOffers.Select(t => new RadiologyScanOfferDto
            {
                Id = t.RadiologyScanId,
                Name = t.RadiologyScan?.Name ?? "",
                Price = t.Price ?? 0,
                Description = t.RadiologyScan?.Description ?? "",
                Duration = "N/A",
                Preparation = "N/A"
            }).ToList();

            var result = new { Center = centerDto, Scans = scans };
            return new ResultResponse<object> { ISucsses = true, Obj = result };
        }

        public async Task<ResultResponse<PaginatedList<object>>> Handle(GetPatientRadiologyBookingsQuery request, CancellationToken cancellationToken)
        {
            var patientId = GetPatientId();
            if (string.IsNullOrEmpty(patientId)) return new ResultResponse<PaginatedList<object>> { ISucsses = false, Message = "Unauthorized" };

            var query = _context.LabBookings
                .Include(b => b.RadiologyCenter)
                .Where(b => b.PatientId == patientId && b.ServiceType == ServiceTypeEnum.Radiology && !b.IsDeleted)
                .AsQueryable();

            var count = await query.CountAsync(cancellationToken);
            var items = await query.OrderByDescending(b => b.ScheduledDate)
                                   .Skip((request.Request.PageNumber - 1) * request.Request.PageSize)
                                   .Take(request.Request.PageSize)
                                   .ToListAsync(cancellationToken);

            var dtos = items.Select(b => new
            {
                b.ID,
                b.RadiologyCenterId,
                CenterName = b.RadiologyCenter?.Name,
                b.Status,
                b.ScheduledDate,
                b.ScheduledTimeSlot,
                b.TotalPrice
            }).Cast<object>().ToList();

            var paginated = new PaginatedList<object>(dtos, count, request.Request.PageNumber, request.Request.PageSize);
            return new ResultResponse<PaginatedList<object>> { ISucsses = true, Obj = paginated };
        }

        public async Task<ResultResponse<object>> Handle(GetPatientRadiologyBookingDetailsQuery request, CancellationToken cancellationToken)
        {
            var patientId = GetPatientId();
            if (string.IsNullOrEmpty(patientId)) return new ResultResponse<object> { ISucsses = false, Message = "Unauthorized" };

            var booking = await _context.LabBookings
                .Include(b => b.RadiologyCenter)
                .Include(b => b.Patient)
                .Include(b => b.Items).ThenInclude(i => i.RadiologyScan)
                .Include(b => b.Items).ThenInclude(i => i.RadiologyResult)
                .FirstOrDefaultAsync(b => b.ID == request.Id && b.PatientId == patientId && !b.IsDeleted, cancellationToken);

            if (booking == null) return new ResultResponse<object> { ISucsses = false, Message = "Appointment not found" };

            return new ResultResponse<object> { ISucsses = true, Obj = _mapper.Map<RadiologyAppointmentDto>(booking) };
        }
    }
}
