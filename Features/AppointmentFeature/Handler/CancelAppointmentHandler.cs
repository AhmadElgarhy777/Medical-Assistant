using DataAccess;
using Features.AppointmentFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Features.AppointmentFeature.Handlers
{
    public class CancelAppointmentHandler : IRequestHandler<CancelAppointmentCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public CancelAppointmentHandler(ApplicationDbContext context) => _context = context;

        public async Task<bool> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            // 1. بندور على الحجز نفسه
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(x => x.ID == request.AppointmentId, cancellationToken);

            if (appointment == null) return false;

            // 2. بنغير حالة الحجز للإلغاء
            appointment.Status = Models.Enums.bookStatusEnum.Cancelled;

            // 3. الجزء السحري: بنفتح الساعة (Slot) تاني عشان تظهر في البحث
            var slot = await _context.DoctorAvilableTimes
                .FirstOrDefaultAsync(x => x.ID == appointment.SlotId, cancellationToken);

            if (slot != null)
            {
                slot.IsBooked = false; // الميعاد رجع متاح يا دكتور!
            }

            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
