using DataAccess;
using Features.PatientFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models;
using System.Linq; // مهم عشان الـ Select

public class GetFullPrescriptionHandler : IRequestHandler<GetFullPrescriptionQuery, PrescriptionFullDetailsDto?>
{
    private readonly ApplicationDbContext _context;

    public GetFullPrescriptionHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PrescriptionFullDetailsDto?> Handle(GetFullPrescriptionQuery request, CancellationToken cancellationToken)
    {
        // هنا بننادي على Prescriptions (اللي احنا لسه مظبطينها في الخطوة 2)
        var data = await _context.Prescriptions
            .Include(p => p.Doctor)
            .Include(p => p.Patient)
            .Include(p => p.items)
            .FirstOrDefaultAsync(p => p.ID.ToString() == request.PrescriptionId.ToString(), cancellationToken);

        if (data == null) return null;

        return new PrescriptionFullDetailsDto
        {
            PrescriptionId = data.ID,
            Diagnosis = data.Diagnosis ?? "بدون تشخيص",
            CreatedAt = data.CraetedAt,
            DoctorName = data.Doctor?.FullName ?? "غير مسجل",
            PatientName = data.Patient?.FullName ?? "غير مسجل",
            Medicines = data.items.Select(m => new MedicineItemDto
            {
                DrugName = m.DrugName,
                Dosage = m.Dos
            }).ToList()
        };
    }
}