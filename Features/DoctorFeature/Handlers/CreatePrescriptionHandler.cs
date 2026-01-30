using DataAccess;
using Features.DoctorFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Features.DoctorFeature.Handlers
{
    public class CreatePrescriptionHandler : IRequestHandler<CreatePrescriptionCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public CreatePrescriptionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
        {
            // 1. إنشاء الروشتة (لاحظ الأسماء مطابقة للموديل بتاعك بالظبط)
            var prescription = new Prescription
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                AppointmentId = request.AppointmentId,
                Diagnosis = request.Diagnosis,
                CraetedAt = DateTime.Now // الاسم المكتوب في الموديل بتاعك
            };

            _context.Prescriptions.Add(prescription); // تأكد إنها بالـ i في الـ DBContext برضه
            await _context.SaveChangesAsync(cancellationToken);

            // 2. إضافة الأدوية (Items)
            if (request.Medicines != null && request.Medicines.Any())
            {
                foreach (var med in request.Medicines)
                {
                    var item = new PrescriptionItem
                    {
                        PresciptionId = prescription.ID, // مربوط بالروشتة
                        DrugName = med.Name,             // الحقل الأول للاسم
                        MedicineName = med.Name,         // الحقل التاني للاسم (عشان ميدي Error)
                        Dos = med.Dosage,                // الجرعة
                        Note = "لا يوجد",                // قيمة افتراضية عشان الـ Nulls
                        Duration = "غير محدد"            // قيمة افتراضية
                    };
                    _context.PrescriptionItems.Add(item);
                }

                // ... كود الحفظ ...

                await _context.SaveChangesAsync(cancellationToken);
            }

            return true;
        }
    }
}