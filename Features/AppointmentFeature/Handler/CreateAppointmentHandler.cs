using DataAccess;
using Features.AppointmentFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Features.AppointmentFeature.Handlers
{
    public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor http;

        public CreateAppointmentHandler(ApplicationDbContext context,IHttpContextAccessor http)
        {
            _context = context;
            this.http = http;
        }

        public async Task<bool> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {

            var patientId = http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // الخطوة 1: نجيب بيانات الساعة المتاحة الأول
            // دي أهم خطوة عشان نعرف الـ DoctorId الحقيقي والوقت
            var slot = await _context.DoctorAvilableTimes
                .FirstOrDefaultAsync(x => x.ID == request.SlotId && x.IsBooked == false, cancellationToken);

            // لو الساعة مش موجودة أو محجوزة أصلاً، ارفض فوراً
            if (slot == null) return false;

            // الخطوة 2: منع التكرار (Validation)
            // بنشوف هل المريض ده عنده حجز "مش ملغي" عند "نفس الدكتور" في "تاريخ النهاردة"
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var isAlreadyBookedToday = await _context.Appointments
                .AnyAsync(x => x.PatientId == patientId
                            && x.DoctorId == slot.DoctorId // استخدمنا id الدكتور من الـ slot اللي جبناه فوق
                            && x.Date == today
                            && x.Status != Models.Enums.bookStatusEnum.Cancelled, cancellationToken);

            // لو لقينا حجز موجود، بنرجع false والسيستم مش بيكمل
            if (isAlreadyBookedToday) return false;

            // الخطوة 3: إنشاء الحجز الجديد
            var appointment = new Appointment
            {
                DoctorId = slot.DoctorId, // لضمان إن الحجز يروح للدكتور الصح
                PatientId = patientId,
                SlotId = slot.ID,
                Date = today,
                StartTime = slot.StartTime,
                Type = request.Type,
                Status = Models.Enums.bookStatusEnum.Pending, // بيبدأ بـ Pending (رقم 0 في الـ Enum بتاعك)
                PaymentStatus = Models.Enums.BookPaymentStatusEnum.NotConfirmed
            };

            // الخطوة 4: تحديث حالة الساعة (عشان مريض تاني مياخدهاش)
            slot.IsBooked = true;

            // الخطوة 5: حفظ في الداتابيز
            _context.Appointments.Add(appointment);

            // الخطوة 6: ربط المريض بالدكتور (لو مش مربوطين قبل كدة)
            var isAlreadyLinked = await _context.DoctorPatients
                .AnyAsync(dp => dp.DoctorId == slot.DoctorId && dp.PatientId == patientId, cancellationToken);

            if (!isAlreadyLinked)
            {
                _context.DoctorPatients.Add(new DoctorPatient
                {
                    DoctorId = slot.DoctorId,
                    PatientId = patientId
                });
            }

            // الخطوة 7: تنفيذ كل التغييرات في خبطة واحدة
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
