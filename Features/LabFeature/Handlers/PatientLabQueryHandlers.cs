using AutoMapper;
using DataAccess;
using Features.LabFeature.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using System.Security.Claims;
using Models.Enums;

namespace Features.LabFeature.Handlers
{
    public class PatientLabQueryHandlers :
        IRequestHandler<GetLabAreasQuery, ResultResponse<object>>,
        IRequestHandler<GetLabsByAreaQuery, ResultResponse<PaginatedList<object>>>,
        IRequestHandler<GetLabDetailsForPatientQuery, ResultResponse<object>>,
        IRequestHandler<SearchLabTestsQuery, ResultResponse<PaginatedList<object>>>,
        IRequestHandler<GetPatientLabBookingsQuery, ResultResponse<PaginatedList<object>>>,
        IRequestHandler<GetPatientLabBookingDetailsQuery, ResultResponse<object>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public PatientLabQueryHandlers(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private string? GetPatientId() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<ResultResponse<object>> Handle(GetLabAreasQuery request, CancellationToken cancellationToken)
        {
            var areas = await _context.Areas.Where(a => !a.IsDeleted).Select(a => new { a.ID, a.Name }).ToListAsync(cancellationToken);
            return new ResultResponse<object> { ISucsses = true, Obj = areas };
        }

        public async Task<ResultResponse<PaginatedList<object>>> Handle(GetLabsByAreaQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Labs
                .Where(l => l.AreaId == request.AreaId && l.Status == ConfrmationStatus.Approved && !l.IsDeleted && l.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Request.Search))
            {
                query = query.Where(l => l.Name.Contains(request.Request.Search));
            }

            var count = await query.CountAsync(cancellationToken);
            var labs = await query.Skip((request.Request.PageNumber - 1) * request.Request.PageSize)
                                  .Take(request.Request.PageSize)
                                  .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<LabDto>>(labs).Cast<object>().ToList();
            var paginated = new PaginatedList<object>(dtos, count, request.Request.PageNumber, request.Request.PageSize);

            return new ResultResponse<PaginatedList<object>> { ISucsses = true, Obj = paginated };
        }

        public async Task<ResultResponse<object>> Handle(GetLabDetailsForPatientQuery request, CancellationToken cancellationToken)
        {
            var lab = await _context.Labs
                .Include(l => l.TestOffers.Where(t => !t.IsDeleted && t.IsAvailable)).ThenInclude(t => t.MedicalTest)
                .FirstOrDefaultAsync(l => l.ID == request.Id && !l.IsDeleted && l.Status == ConfrmationStatus.Approved, cancellationToken);

            if (lab == null) return new ResultResponse<object> { ISucsses = false, Message = "Lab not found" };

            var labDto = _mapper.Map<LabDto>(lab);
            var tests = lab.TestOffers.Select(t => new LabTestDto
            {
                Id = t.MedicalTestId,
                Name = t.MedicalTest?.Name ?? "",
                Category = t.MedicalTest?.Category ?? "",
                Price = t.Price ?? 0,
                EstimatedTime = t.MedicalTest?.TurnaroundHours.ToString() ?? "",
                Description = t.MedicalTest?.Description ?? "",
                IsAvailable = t.IsAvailable
            }).ToList();

            var result = new { Lab = labDto, Tests = tests };
            return new ResultResponse<object> { ISucsses = true, Obj = result };
        }

        public async Task<ResultResponse<PaginatedList<object>>> Handle(SearchLabTestsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.LabTestOffers
                .Include(t => t.MedicalTest)
                .Include(t => t.Lab)
                .Where(t => !t.IsDeleted && t.IsAvailable && t.Lab.Status == ConfrmationStatus.Approved && !t.Lab.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Query))
            {
                query = query.Where(t => t.MedicalTest!.Name.Contains(request.Query));
            }

            var count = await query.CountAsync(cancellationToken);
            var items = await query.Skip((request.Request.PageNumber - 1) * request.Request.PageSize)
                                   .Take(request.Request.PageSize)
                                   .ToListAsync(cancellationToken);

            var results = items.Select(t => new
            {
                TestId = t.MedicalTestId,
                TestName = t.MedicalTest?.Name,
                LabId = t.LabId,
                LabName = t.Lab?.Name,
                Price = t.Price
            }).Cast<object>().ToList();

            var paginated = new PaginatedList<object>(results, count, request.Request.PageNumber, request.Request.PageSize);
            return new ResultResponse<PaginatedList<object>> { ISucsses = true, Obj = paginated };
        }

        public async Task<ResultResponse<PaginatedList<object>>> Handle(GetPatientLabBookingsQuery request, CancellationToken cancellationToken)
        {
            var patientId = GetPatientId();
            if (string.IsNullOrEmpty(patientId)) return new ResultResponse<PaginatedList<object>> { ISucsses = false, Message = "Unauthorized" };

            var query = _context.LabBookings
                .Include(b => b.Lab)
                .Where(b => b.PatientId == patientId && b.ServiceType == ServiceTypeEnum.Lab && !b.IsDeleted)
                .AsQueryable();

            var count = await query.CountAsync(cancellationToken);
            var items = await query.OrderByDescending(b => b.ScheduledDate)
                                   .Skip((request.Request.PageNumber - 1) * request.Request.PageSize)
                                   .Take(request.Request.PageSize)
                                   .ToListAsync(cancellationToken);

            var dtos = items.Select(b => new
            {
                b.ID,
                b.LabId,
                LabName = b.Lab?.Name,
                b.Status,
                b.ScheduledDate,
                b.ScheduledTimeSlot,
                b.TotalPrice
            }).Cast<object>().ToList();

            var paginated = new PaginatedList<object>(dtos, count, request.Request.PageNumber, request.Request.PageSize);
            return new ResultResponse<PaginatedList<object>> { ISucsses = true, Obj = paginated };
        }

        public async Task<ResultResponse<object>> Handle(GetPatientLabBookingDetailsQuery request, CancellationToken cancellationToken)
        {
            var patientId = GetPatientId();
            if (string.IsNullOrEmpty(patientId)) return new ResultResponse<object> { ISucsses = false, Message = "Unauthorized" };

            var booking = await _context.LabBookings
                .Include(b => b.Lab)
                .Include(b => b.Patient)
                .Include(b => b.Items).ThenInclude(i => i.MedicalTest)
                .Include(b => b.Items).ThenInclude(i => i.Result)
                .FirstOrDefaultAsync(b => b.ID == request.Id && b.PatientId == patientId && !b.IsDeleted, cancellationToken);

            if (booking == null) return new ResultResponse<object> { ISucsses = false, Message = "Booking not found" };

            return new ResultResponse<object> { ISucsses = true, Obj = _mapper.Map<LabBookingDto>(booking) };
        }
    }
}
