using MediatR;
using Features;
using Features.RadiologyFeature.Commands;
using Features.RadiologyFeature.Queries;
using DataAccess;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Features.RadiologyFeature.Handlers
{
    public class RadiologyScheduleHandlers : 
        IRequestHandler<AddRadiologyScheduleCommand, ResultResponse<string>>,
        IRequestHandler<UpdateRadiologyScheduleCommand, ResultResponse<string>>,
        IRequestHandler<DeleteRadiologyScheduleCommand, ResultResponse<string>>,
        IRequestHandler<GetRadiologyScheduleQuery, ResultResponse<object>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RadiologyScheduleHandlers(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string? GetRadiologyEmail() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public async Task<ResultResponse<string>> Handle(AddRadiologyScheduleCommand request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };

            var schedule = new RadiologySchedule
            {
                RadiologyCenterId = center.ID,
                DayOfWeek = request.DayOfWeek,
                FromTime = request.FromTime,
                ToTime = request.ToTime,
                IsAvailable = true
            };

            _context.RadiologySchedules.Add(schedule);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Obj = schedule.ID };
        }

        public async Task<ResultResponse<string>> Handle(UpdateRadiologyScheduleCommand request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };

            var schedule = await _context.RadiologySchedules.FirstOrDefaultAsync(s => s.ID == request.Id && s.RadiologyCenterId == center.ID && !s.IsDeleted, cancellationToken);
            if (schedule == null) return new ResultResponse<string> { ISucsses = false, Message = "Schedule not found" };

            schedule.DayOfWeek = request.DayOfWeek;
            schedule.FromTime = request.FromTime;
            schedule.ToTime = request.ToTime;
            schedule.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Message = "Updated Successfully" };
        }

        public async Task<ResultResponse<string>> Handle(DeleteRadiologyScheduleCommand request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };

            var schedule = await _context.RadiologySchedules.FirstOrDefaultAsync(s => s.ID == request.Id && s.RadiologyCenterId == center.ID && !s.IsDeleted, cancellationToken);
            if (schedule == null) return new ResultResponse<string> { ISucsses = false, Message = "Schedule not found" };

            schedule.IsDeleted = true;
            schedule.DeletedAT = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Message = "Deleted Successfully" };
        }

        public async Task<ResultResponse<object>> Handle(GetRadiologyScheduleQuery request, CancellationToken cancellationToken)
        {
            var email = GetRadiologyEmail();
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
            if (center == null) return new ResultResponse<object> { ISucsses = false, Message = "Radiology Center not found" };

            var schedules = await _context.RadiologySchedules
                .Where(s => s.RadiologyCenterId == center.ID && !s.IsDeleted)
                .Select(s => new {
                    s.ID,
                    s.DayOfWeek,
                    FromTime = s.FromTime.ToString(@"hh\:mm"),
                    ToTime = s.ToTime.ToString(@"hh\:mm"),
                    s.IsAvailable
                }).ToListAsync(cancellationToken);

            return new ResultResponse<object> { ISucsses = true, Obj = schedules };
        }
    }
}
