using DataAccess;
using Features.DoctorFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;

namespace Features.DoctorFeature.Handlers
{
    public class GetDoctorStatsHandler : IRequestHandler<GetDoctorStatsQuery, DoctorStatsDTO>
    {
        private readonly ApplicationDbContext _context;

        public GetDoctorStatsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorStatsDTO> Handle(GetDoctorStatsQuery request, CancellationToken cancellationToken)
        {
            // 1. تاريخ النهاردة
            var today = DateOnly.FromDateTime(DateTime.Now);

            // 2. نجيب كل حجوزات النهاردة للدكتور ده بس
            var todayAppointments = await _context.Appointments
                .Where(x => x.DoctorId == request.DoctorId && x.Date == today)
                .ToListAsync(cancellationToken);

            // 3. نوزع الأرقام حسب الحالة (Status)
            return new DoctorStatsDTO
            {
                TotalToday = todayAppointments.Count,   

                // بنعد الحالات بناءً على الـ Enum اللي عندك
                CompletedToday = todayAppointments.Count(x => x.Status == Models.Enums.bookStatusEnum.Completed),
                PendingToday = todayAppointments.Count(x => x.Status == Models.Enums.bookStatusEnum.Pending),
                CancelledToday = todayAppointments.Count(x => x.Status == Models.Enums.bookStatusEnum.Cancelled)
            };
        }
    }
}
