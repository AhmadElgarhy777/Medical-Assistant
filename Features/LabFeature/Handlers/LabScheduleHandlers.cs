using MediatR;
using Features;
using Features.LabFeature.Commands;
using Features.LabFeature.Queries;
using DataAccess;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Features.LabFeature.Handlers
{
    public class LabScheduleHandlers : 
        IRequestHandler<AddLabScheduleCommand, ResultResponse<string>>,
        IRequestHandler<UpdateLabScheduleCommand, ResultResponse<string>>,
        IRequestHandler<DeleteLabScheduleCommand, ResultResponse<string>>,
        IRequestHandler<GetLabScheduleQuery, ResultResponse<object>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LabScheduleHandlers(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string? GetLabEmail() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public async Task<ResultResponse<string>> Handle(AddLabScheduleCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            var schedule = new LabSchedule
            {
                LabId = lab.ID,
                DayOfWeek = request.DayOfWeek,
                FromTime = request.FromTime,
                ToTime = request.ToTime,
                IsAvailable = true
            };

            _context.LabSchedules.Add(schedule);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Obj = schedule.ID };
        }

        public async Task<ResultResponse<string>> Handle(UpdateLabScheduleCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            var schedule = await _context.LabSchedules.FirstOrDefaultAsync(s => s.ID == request.Id && s.LabId == lab.ID && !s.IsDeleted, cancellationToken);
            if (schedule == null) return new ResultResponse<string> { ISucsses = false, Message = "Schedule not found" };

            schedule.DayOfWeek = request.DayOfWeek;
            schedule.FromTime = request.FromTime;
            schedule.ToTime = request.ToTime;
            schedule.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Message = "Updated Successfully" };
        }

        public async Task<ResultResponse<string>> Handle(DeleteLabScheduleCommand request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };

            var schedule = await _context.LabSchedules.FirstOrDefaultAsync(s => s.ID == request.Id && s.LabId == lab.ID && !s.IsDeleted, cancellationToken);
            if (schedule == null) return new ResultResponse<string> { ISucsses = false, Message = "Schedule not found" };

            schedule.IsDeleted = true;
            schedule.DeletedAT = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Message = "Deleted Successfully" };
        }

        public async Task<ResultResponse<object>> Handle(GetLabScheduleQuery request, CancellationToken cancellationToken)
        {
            var email = GetLabEmail();
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.Email == email, cancellationToken);
            if (lab == null) return new ResultResponse<object> { ISucsses = false, Message = "Lab not found" };

            var schedules = await _context.LabSchedules
                .Where(s => s.LabId == lab.ID && !s.IsDeleted)
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
