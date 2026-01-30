using DataAccess;
using Features.AppointmentFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Features.AppointmentFeature.Handlers
{
    public class UpdateAppointmentStatusHandler : IRequestHandler<UpdateAppointmentStatusCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public UpdateAppointmentStatusHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateAppointmentStatusCommand request, CancellationToken cancellationToken)
        {
            // 1. بندور على الحجز في الداتابيز
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(x => x.ID == request.AppointmentId, cancellationToken);

            if (appointment == null) return false;

            // 2. بنحدث الحالة للحالة الجديدة اللي الدكتور بعتها
            appointment.Status = request.NewStatus;

            // 3. بنسمّع في الداتابيز
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
