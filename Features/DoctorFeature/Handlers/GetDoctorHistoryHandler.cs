using DataAccess;
using Features.DoctorFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;

namespace Features.DoctorFeature.Handlers
{
    public class GetDoctorHistoryHandler : IRequestHandler<GetDoctorHistoryQuery, List<DoctorAppointmentsDTO>>
    {
        private readonly ApplicationDbContext _context;

        // 1. بنحقن الداتابيز عشان نعرف نكلمها
        public GetDoctorHistoryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DoctorAppointmentsDTO>> Handle(GetDoctorHistoryQuery request, CancellationToken cancellationToken)
        {
            var history = await _context.Appointments
                .Include(x => x.Patient) // لازم نجيب بيانات المريض عشان نعرف اسمه
                .Where(x => x.DoctorId == request.DoctorId
                        && (x.Status == Models.Enums.bookStatusEnum.Completed
                            || x.Status == Models.Enums.bookStatusEnum.Cancelled))
                .OrderByDescending(x => x.Date) // بنرتب من الأحدث للأقدم
                .ToListAsync(cancellationToken);

            // 3. بنحول البيانات لـ DTO عشان تطلع للـ Swagger بشكل نضيف
            var result = history.Select(x => new DoctorAppointmentsDTO
            {
                PatientName = x.Patient.FullName,
                AppointmentDate = x.Date.ToString("yyyy-MM-dd"),
                StartTime = x.StartTime.ToString("HH:mm"),
                Status = x.Status.ToString(), // هيطلع لك كلمة "Completed" أو "Cancelled"
                BookingType = x.Type.ToString() // هيطلع لك "Clinic" أو "Online"
            }).ToList();

            return result;
        }
    }
}
