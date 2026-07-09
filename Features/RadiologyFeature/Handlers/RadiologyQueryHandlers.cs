using AutoMapper;
using DataAccess;
using Features.RadiologyFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using System.Security.Claims;
using Models.Enums;

namespace Features.RadiologyFeature.Handlers
{
    public class RadiologyQueryHandlers :
        IRequestHandler<GetRadiologyDashboardQuery, ResultResponse<object>>,
        IRequestHandler<GetRadiologyProfileQuery, ResultResponse<object>>,
        IRequestHandler<GetRadiologyScansQuery, ResultResponse<PaginatedList<object>>>,
        IRequestHandler<GetRadiologyAppointmentsQuery, ResultResponse<PaginatedList<object>>>,
        IRequestHandler<GetRadiologyAppointmentDetailsQuery, ResultResponse<object>>,
        IRequestHandler<GetRadiologyReportQuery, ResultResponse<object>>,
        IRequestHandler<GetRadiologyAnalyticsQuery, ResultResponse<object>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public RadiologyQueryHandlers(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private string? GetRadiologyEmail() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public async Task<ResultResponse<object>> Handle(GetRadiologyDashboardQuery request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<object> { ISucsses = false, Message = "Radiology Center not found" };

            var today = DateTime.Today;
            var bookingsQuery = _context.LabBookings
                .Include(b => b.Patient)
                .Where(b => b.RadiologyCenterId == center.ID && !b.IsDeleted);

            var todayAppointmentsCount = await bookingsQuery.CountAsync(b => b.ScheduledDate.Date == today, cancellationToken);
            var pendingAppointmentsCount = await bookingsQuery.CountAsync(b => b.Status == LabBookingStatusEnum.Pending || b.Status == LabBookingStatusEnum.PendingPayment, cancellationToken);
            var completedAppointmentsCount = await bookingsQuery.CountAsync(b => b.Status == LabBookingStatusEnum.Completed, cancellationToken);
            
            var revenue = await bookingsQuery.Where(b => b.IsPaid).SumAsync(b => b.TotalPrice, cancellationToken);

            var recentActivity = await bookingsQuery
                .OrderByDescending(b => b.ScheduledDate)
                .Take(5)
                .ToListAsync(cancellationToken);

            var dashboard = new RadiologyDashboardDto
            {
                TodaysAppointments = todayAppointmentsCount,
                PendingAppointments = pendingAppointmentsCount,
                CompletedAppointments = completedAppointmentsCount,
                Revenue = revenue,
                RecentActivity = _mapper.Map<List<RadiologyAppointmentDto>>(recentActivity)
            };

            return new ResultResponse<object> { ISucsses = true, Obj = dashboard };
        }

        public async Task<ResultResponse<object>> Handle(GetRadiologyProfileQuery request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<object> { ISucsses = false, Message = "Radiology Center not found" };

            return new ResultResponse<object> { ISucsses = true, Obj = _mapper.Map<RadiologyCenterDto>(center) };
        }

        public async Task<ResultResponse<PaginatedList<object>>> Handle(GetRadiologyScansQuery request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<PaginatedList<object>> { ISucsses = false, Message = "Radiology Center not found" };

            var query = _context.RadiologyCenterScans
                .Include(t => t.RadiologyScan)
                .Where(t => t.RadiologyCenterId == center.ID && !t.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Request.Search))
            {
                query = query.Where(t => t.RadiologyScan!.Name.Contains(request.Request.Search));
            }

            query = request.Request.SortDirection.ToLower() == "desc" 
                ? query.OrderByDescending(t => t.RadiologyScan!.Name) 
                : query.OrderBy(t => t.RadiologyScan!.Name);

            var count = await query.CountAsync(cancellationToken);
            var items = await query.Skip((request.Request.PageNumber - 1) * request.Request.PageSize)
                                   .Take(request.Request.PageSize)
                                   .ToListAsync(cancellationToken);

            var dtos = items.Select(t => new RadiologyScanOfferDto
            {
                Id = t.RadiologyScanId,
                Name = t.RadiologyScan?.Name ?? "",
                Price = t.Price ?? 0,
                Description = t.RadiologyScan?.Description ?? "",
                Duration = "N/A",
                Preparation = "N/A"
            }).Cast<object>().ToList();

            var paginatedList = new PaginatedList<object>(dtos, count, request.Request.PageNumber, request.Request.PageSize);
            return new ResultResponse<PaginatedList<object>> { ISucsses = true, Obj = paginatedList };
        }

        public async Task<ResultResponse<PaginatedList<object>>> Handle(GetRadiologyAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<PaginatedList<object>> { ISucsses = false, Message = "Radiology Center not found" };

            var query = _context.LabBookings
                .Include(b => b.Patient)
                .Include(b => b.Items).ThenInclude(i => i.RadiologyScan)
                .Where(b => b.RadiologyCenterId == center.ID && !b.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<LabBookingStatusEnum>(request.Status, true, out var statusEnum))
            {
                query = query.Where(b => b.Status == statusEnum);
            }

            if (!string.IsNullOrEmpty(request.Request.Search))
            {
                query = query.Where(b => b.Patient.FullName.Contains(request.Request.Search));
            }

            query = request.Request.SortDirection.ToLower() == "desc" 
                ? query.OrderByDescending(b => b.ScheduledDate) 
                : query.OrderBy(b => b.ScheduledDate);

            var count = await query.CountAsync(cancellationToken);
            var items = await query.Skip((request.Request.PageNumber - 1) * request.Request.PageSize)
                                   .Take(request.Request.PageSize)
                                   .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<RadiologyAppointmentDto>>(items).Cast<object>().ToList();
            var paginatedList = new PaginatedList<object>(dtos, count, request.Request.PageNumber, request.Request.PageSize);

            return new ResultResponse<PaginatedList<object>> { ISucsses = true, Obj = paginatedList };
        }

        public async Task<ResultResponse<object>> Handle(GetRadiologyAppointmentDetailsQuery request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<object> { ISucsses = false, Message = "Radiology Center not found" };

            var booking = await _context.LabBookings
                .Include(b => b.Patient)
                .Include(b => b.Items).ThenInclude(i => i.RadiologyScan)
                .Include(b => b.Items).ThenInclude(i => i.RadiologyResult)
                .FirstOrDefaultAsync(b => b.ID == request.Id && b.RadiologyCenterId == center.ID && !b.IsDeleted, cancellationToken);

            if (booking == null) return new ResultResponse<object> { ISucsses = false, Message = "Appointment not found" };

            return new ResultResponse<object> { ISucsses = true, Obj = _mapper.Map<RadiologyAppointmentDto>(booking) };
        }

        public async Task<ResultResponse<object>> Handle(GetRadiologyReportQuery request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<object> { ISucsses = false, Message = "Radiology Center not found" };

            var booking = await _context.LabBookings
                .Include(b => b.Items).ThenInclude(i => i.RadiologyResult)
                .FirstOrDefaultAsync(b => b.ID == request.Id && b.RadiologyCenterId == center.ID && !b.IsDeleted, cancellationToken);

            if (booking == null) return new ResultResponse<object> { ISucsses = false, Message = "Appointment not found" };

            var item = booking.Items.FirstOrDefault();
            if (item != null && item.RadiologyResult != null && !item.RadiologyResult.IsDeleted)
            {
                return new ResultResponse<object> { ISucsses = true, Obj = new { item.RadiologyResult.ReportFileUrl, item.RadiologyResult.ImagesUrls, item.RadiologyResult.DoctorNotes, item.RadiologyResult.ReportedAt } };
            }

            return new ResultResponse<object> { ISucsses = false, Message = "No report found" };
        }

      

        public async Task<ResultResponse<object>> Handle(GetRadiologyAnalyticsQuery request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<object> { ISucsses = false, Message = "Radiology Center not found" };

            var sinceDate = request.Type.ToLower() switch
            {
                "daily" => DateTime.Today,
                "weekly" => DateTime.Today.AddDays(-7),
                "monthly" => DateTime.Today.AddMonths(-1),
                _ => DateTime.MinValue
            };

            var completedBookings = await _context.LabBookings
                .Where(b => b.RadiologyCenterId == center.ID && !b.IsDeleted && b.Status == LabBookingStatusEnum.Completed && b.ScheduledDate >= sinceDate)
                .ToListAsync(cancellationToken);

            var report = new
            {
                ReportType = request.Type,
                PeriodStart = sinceDate,
                PeriodEnd = DateTime.Now,
                TotalCompletedAppointments = completedBookings.Count,
                TotalRevenue = completedBookings.Sum(b => b.TotalPrice)
            };

            return new ResultResponse<object> { ISucsses = true, Obj = report };
        }
    }
}
