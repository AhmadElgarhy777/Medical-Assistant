using AutoMapper;
using DataAccess;
using Features.LabFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using System.Security.Claims;
using Models.Enums;

namespace Features.LabFeature.Handlers
{
    public class LabQueryHandlers :
        IRequestHandler<GetLabDashboardQuery, ResultResponse<object>>,
        IRequestHandler<GetLabProfileQuery, ResultResponse<object>>,
        IRequestHandler<GetLabTestsQuery, ResultResponse<PaginatedList<object>>>,
        IRequestHandler<GetLabBookingsQuery, ResultResponse<PaginatedList<object>>>,
        IRequestHandler<GetLabBookingDetailsQuery, ResultResponse<object>>,
        IRequestHandler<GetLabResultQuery, ResultResponse<object>>,
        IRequestHandler<GetLabReportQuery, ResultResponse<object>>,
        IRequestHandler<GetHomeCollectionRequestsQuery, ResultResponse<PaginatedList<object>>>,
        IRequestHandler<GetHomeCollectionRequestDetailsQuery, ResultResponse<object>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public LabQueryHandlers(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private string? GetLabEmail() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public async Task<ResultResponse<object>> Handle(GetLabDashboardQuery request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<object> { ISucsses = false, Message = "Lab not found" };

            var today = DateTime.Today;
            var bookingsQuery = _context.LabBookings
                .Include(b => b.Patient)
                .Where(b => b.LabId == lab.ID && !b.IsDeleted);

            var todayBookingsCount = await bookingsQuery.CountAsync(b => b.ScheduledDate.Date == today, cancellationToken);
            var pendingBookingsCount = await bookingsQuery.CountAsync(b => b.Status == LabBookingStatusEnum.Pending || b.Status == LabBookingStatusEnum.PendingPayment, cancellationToken);
            var completedBookingsCount = await bookingsQuery.CountAsync(b => b.Status == LabBookingStatusEnum.Completed, cancellationToken);
            
            var revenue = await bookingsQuery.Where(b => b.IsPaid).SumAsync(b => b.TotalPrice, cancellationToken);
            var homeCollectionCount = await bookingsQuery.CountAsync(b => b.VisitType == VisitTypeEnum.HomeCollection, cancellationToken);

            var recentActivity = await bookingsQuery
                .OrderByDescending(b => b.ScheduledDate)
                .Take(5)
                .ToListAsync(cancellationToken);

            var dashboard = new LabDashboardDto
            {
                TodaysBookings = todayBookingsCount,
                PendingBookings = pendingBookingsCount,
                CompletedBookings = completedBookingsCount,
                Revenue = revenue,
                HomeCollectionCount = homeCollectionCount,
                RecentActivity = _mapper.Map<List<LabBookingDto>>(recentActivity)
            };

            return new ResultResponse<object> { ISucsses = true, Obj = dashboard };
        }

        public async Task<ResultResponse<object>> Handle(GetLabProfileQuery request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<object> { ISucsses = false, Message = "Lab not found" };

            return new ResultResponse<object> { ISucsses = true, Obj = _mapper.Map<LabDto>(lab) };
        }

        public async Task<ResultResponse<PaginatedList<object>>> Handle(GetLabTestsQuery request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<PaginatedList<object>> { ISucsses = false, Message = "Lab not found" };

            var query = _context.LabTestOffers
                .Include(t => t.MedicalTest)
                .Where(t => t.LabId == lab.ID && !t.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Request.Search))
            {
                query = query.Where(t => t.MedicalTest!.Name.Contains(request.Request.Search));
            }

            // Implement sorting logic if needed
            // Default sort:
            query = request.Request.SortDirection.ToLower() == "desc" 
                ? query.OrderByDescending(t => t.MedicalTest!.Name) 
                : query.OrderBy(t => t.MedicalTest!.Name);

            var count = await query.CountAsync(cancellationToken);
            var items = await query.Skip((request.Request.PageNumber - 1) * request.Request.PageSize)
                                   .Take(request.Request.PageSize)
                                   .ToListAsync(cancellationToken);

            var testDtos = items.Select(t => new LabTestDto
            {
                Id = t.MedicalTestId,
                Name = t.MedicalTest?.Name ?? "",
                Category = t.MedicalTest?.Category ?? "",
                Price = t.Price ?? 0,
                EstimatedTime = t.MedicalTest?.TurnaroundHours.ToString() ?? "",
                Description = t.MedicalTest?.Description ?? "",
                IsAvailable = t.IsAvailable
            }).Cast<object>().ToList();

            var paginatedList = new PaginatedList<object>(testDtos, count, request.Request.PageNumber, request.Request.PageSize);
            return new ResultResponse<PaginatedList<object>> { ISucsses = true, Obj = paginatedList };
        }

        public async Task<ResultResponse<PaginatedList<object>>> Handle(GetLabBookingsQuery request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<PaginatedList<object>> { ISucsses = false, Message = "Lab not found" };

            var query = _context.LabBookings
                .Include(b => b.Patient)
                .Where(b => b.LabId == lab.ID && !b.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<LabBookingStatusEnum>(request.Status, true, out var statusEnum))
            {
                query = query.Where(b => b.Status == statusEnum);
            }

            if (!string.IsNullOrEmpty(request.Request.Search))
            {
                query = query.Where(b => b.Patient != null && b.Patient.FullName.Contains(request.Request.Search));
            }

            query = request.Request.SortDirection.ToLower() == "desc" 
                ? query.OrderByDescending(b => b.ScheduledDate) 
                : query.OrderBy(b => b.ScheduledDate);

            var count = await query.CountAsync(cancellationToken);
            var items = await query.Skip((request.Request.PageNumber - 1) * request.Request.PageSize)
                                   .Take(request.Request.PageSize)
                                   .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<LabBookingDto>>(items).Cast<object>().ToList();
            var paginatedList = new PaginatedList<object>(dtos, count, request.Request.PageNumber, request.Request.PageSize);

            return new ResultResponse<PaginatedList<object>> { ISucsses = true, Obj = paginatedList };
        }

        public async Task<ResultResponse<object>> Handle(GetLabBookingDetailsQuery request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<object> { ISucsses = false, Message = "Lab not found" };

            var booking = await _context.LabBookings
                .Include(b => b.Patient)
                .Include(b => b.Items).ThenInclude(i => i.MedicalTest)
                .Include(b => b.Items).ThenInclude(i => i.Result)
                .FirstOrDefaultAsync(b => b.ID == request.Id && b.LabId == lab.ID && !b.IsDeleted, cancellationToken);

            if (booking == null) return new ResultResponse<object> { ISucsses = false, Message = "Booking not found" };

            return new ResultResponse<object> { ISucsses = true, Obj = _mapper.Map<LabBookingDto>(booking) };
        }

        public async Task<ResultResponse<PaginatedList<object>>> Handle(GetHomeCollectionRequestsQuery request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<PaginatedList<object>> { ISucsses = false, Message = "Lab not found" };

            var query = _context.LabBookings
                .Include(b => b.Patient)
                .Where(b => b.LabId == lab.ID && b.VisitType == VisitTypeEnum.HomeCollection && !b.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<LabBookingStatusEnum>(request.Status, true, out var statusEnum))
            {
                query = query.Where(b => b.Status == statusEnum);
            }

            if (!string.IsNullOrEmpty(request.Request.Search))
            {
                query = query.Where(b => b.Patient.FullName.Contains(request.Request.Search) || (b.HomeAddress != null && b.HomeAddress.Contains(request.Request.Search)));
            }

            query = request.Request.SortDirection.ToLower() == "desc" 
                ? query.OrderByDescending(b => b.ScheduledDate) 
                : query.OrderBy(b => b.ScheduledDate);

            var count = await query.CountAsync(cancellationToken);
            var items = await query.Skip((request.Request.PageNumber - 1) * request.Request.PageSize)
                                   .Take(request.Request.PageSize)
                                   .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<LabBookingDto>>(items).Cast<object>().ToList();
            var paginatedList = new PaginatedList<object>(dtos, count, request.Request.PageNumber, request.Request.PageSize);

            return new ResultResponse<PaginatedList<object>> { ISucsses = true, Obj = paginatedList };
        }

        public async Task<ResultResponse<object>> Handle(GetHomeCollectionRequestDetailsQuery request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<object> { ISucsses = false, Message = "Lab not found" };

            var booking = await _context.LabBookings
                .Include(b => b.Patient)
                .Include(b => b.Items).ThenInclude(i => i.MedicalTest)
                .FirstOrDefaultAsync(b => b.ID == request.Id && b.LabId == lab.ID && b.VisitType == VisitTypeEnum.HomeCollection && !b.IsDeleted, cancellationToken);

            if (booking == null) return new ResultResponse<object> { ISucsses = false, Message = "Home Collection Request not found" };

            return new ResultResponse<object> { ISucsses = true, Obj = _mapper.Map<LabBookingDto>(booking) };
        }

        public async Task<ResultResponse<object>> Handle(GetLabResultQuery request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<object> { ISucsses = false, Message = "Lab not found" };

            var booking = await _context.LabBookings
                .Include(b => b.Items).ThenInclude(i => i.Result)
                .FirstOrDefaultAsync(b => b.ID == request.Id && b.LabId == lab.ID && !b.IsDeleted, cancellationToken);

            if (booking == null) return new ResultResponse<object> { ISucsses = false, Message = "Booking not found" };

            var item = booking.Items.FirstOrDefault();
            if (item != null && item.Result != null && !item.Result.IsDeleted)
            {
                return new ResultResponse<object> { ISucsses = true, Obj = new { item.Result.ResultFileUrl, item.Result.ResultValuesJson, item.Result.DoctorNotes } };
            }

            return new ResultResponse<object> { ISucsses = false, Message = "No result found" };
        }

        //public async Task<ResultResponse<PaginatedList<object>>> Handle(GetLabNotificationsQuery request, CancellationToken cancellationToken)
        //{
        //    var email = GetLabEmail();
        //    var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
        //    if (lab == null) return new ResultResponse<PaginatedList<object>> { ISucsses = false, Message = "Lab not found" };

        //    var query = _context.Notifications.Where(n => n.UserId == email); // Notifications tie to Email/UserId
            
        //    var count = await query.CountAsync(cancellationToken);
        //    var items = await query.OrderByDescending(n => n.CreatedAt)
        //                           .Skip((request.Request.PageNumber - 1) * request.Request.PageSize)
        //                           .Take(request.Request.PageSize)
        //                           .ToListAsync(cancellationToken);

        //    var paginatedList = new PaginatedList<object>(items.Cast<object>().ToList(), count, request.Request.PageNumber, request.Request.PageSize);
        //    return new ResultResponse<PaginatedList<object>> { ISucsses = true, Obj = paginatedList };
        //}

        public async Task<ResultResponse<object>> Handle(GetLabReportQuery request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<object> { ISucsses = false, Message = "Lab not found" };

            // Very basic report stub based on Type (Daily, Weekly, Monthly)
            var sinceDate = request.Type.ToLower() switch
            {
                "daily" => DateTime.Today,
                "weekly" => DateTime.Today.AddDays(-7),
                "monthly" => DateTime.Today.AddMonths(-1),
                _ => DateTime.MinValue
            };

            var completedBookings = await _context.LabBookings
                .Where(b => b.LabId == lab.ID && !b.IsDeleted && b.Status == LabBookingStatusEnum.Completed && b.ScheduledDate >= sinceDate)
                .ToListAsync(cancellationToken);

            var report = new
            {
                ReportType = request.Type,
                PeriodStart = sinceDate,
                PeriodEnd = DateTime.Now,
                TotalCompletedBookings = completedBookings.Count,
                TotalRevenue = completedBookings.Sum(b => b.TotalPrice)
            };

            return new ResultResponse<object> { ISucsses = true, Obj = report };
        }
    }
}
