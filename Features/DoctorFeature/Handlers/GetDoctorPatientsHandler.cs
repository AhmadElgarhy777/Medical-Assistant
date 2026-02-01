using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using DataAccess;
using Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Features.DoctorFeature.Queries;

namespace Features.DoctorFeature.Handlers
{
    public class GetDoctorPatientsHandler : IRequestHandler<GetDoctorPatientsQuery, List<PatientDTO>>
    {
        private readonly ApplicationDbContext _context;

        public GetDoctorPatientsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PatientDTO>> Handle(GetDoctorPatientsQuery request, CancellationToken cancellationToken)
        {
            // بنروح لجدول الربط (DoctorPatients) 
            // وبنقول له هات كل السجلات اللي تخص الدكتور ده
            var patients = await _context.DoctorPatients
                .Where(dp => dp.DoctorId == request.DoctorId)
                .Select(dp => new PatientDTO // بنحول الداتا لـ DTO عشان نعرضها
                {
                    Id = dp.Patient.ID,
                    FullName = dp.Patient.FullName,
                    Email = dp.Patient.Email,
                    Gender = dp.Patient.Gender.ToString(),
                    Address = dp.Patient.Address,
                    City = dp.Patient.City,
                    BloodType=dp.Patient.BloodType
                    
                })
                .ToListAsync(cancellationToken);

            return patients;
        }
    }
}
