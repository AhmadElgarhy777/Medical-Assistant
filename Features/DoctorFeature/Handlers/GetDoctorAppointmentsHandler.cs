using DataAccess;
using Features.DoctorFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Features.DoctorFeature.Handlers
{
    public class GetDoctorAppointmentsHandler : IRequestHandler<GetDoctorAppointmentsQuery, List<DoctorAppointmentsDTO>>
    {
        private readonly ApplicationDbContext _context;

        public GetDoctorAppointmentsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DoctorAppointmentsDTO>> Handle(GetDoctorAppointmentsQuery request, CancellationToken cancellationToken)
        {
            // 1. بنحدد تاريخ النهاردة عشان نعرض مواعيد اليوم فقط
            var today = DateOnly.FromDateTime(DateTime.Now);

            // 2. بنجيب الحجوزات مع الفلترة الذكية
            var appointments = await _context.Appointments
                .Include(x => x.Patient)
                .Where(x => x.DoctorId == request.DoctorId
                        && x.Date == today // تاريخ النهاردة بس
                        && (x.Status == Models.Enums.bookStatusEnum.Pending
                            || x.Status == Models.Enums.bookStatusEnum.Confirmed)) // الحالات الجديدة فقط
                .OrderBy(x => x.StartTime) // ترتيب المواعيد من الأقرب للأبعد
                .ToListAsync(cancellationToken);

            // 3. تحويل البيانات لشكل الـ DTO
            var result = appointments.Select(x => new DoctorAppointmentsDTO
            {
                PatientName = x.Patient.FullName,
                AppointmentDate = x.Date.ToString("yyyy-MM-dd"),
                StartTime = x.StartTime.ToString("HH:mm"),
                Status = x.Status.ToString(),
                BookingType = x.Type.ToString()
            }).ToList();

            return result;
        }
    }
}