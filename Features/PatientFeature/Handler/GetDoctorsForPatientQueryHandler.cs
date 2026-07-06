using DataAccess.Repositry.IRepositry;
using Features.PatientFeature.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Handler
{
    public class GetDoctorsForPatientQueryHandler : IRequestHandler<GetDoctorsForPatientQuery, ResultResponse<List<MyDoctorsDto>>>
    {
        private readonly IDoctorPatientRepositry doctorPatientRepositry;

        public GetDoctorsForPatientQueryHandler(IDoctorPatientRepositry doctorPatientRepositry)
        {
            this.doctorPatientRepositry = doctorPatientRepositry;
        }
        public async Task<ResultResponse<List<MyDoctorsDto>>> Handle(GetDoctorsForPatientQuery request, CancellationToken cancellationToken)
        {
            var doctors = await doctorPatientRepositry.GetTable().Select(e =>
            new MyDoctorsDto
            {
                DoctorID = e.DoctorId,
                DoctorName = e.Doctor.FullName,
                DoctorImg = e.Doctor.Img,
                DoctorSpecilization = e.Doctor.Specialization.Name,
                AppoinmentCount = e.Doctor.Appointments.Count(),

            }
            ).ToListAsync(cancellationToken);

            return new ResultResponse<List<MyDoctorsDto>>
            {
                ISucsses = true,
                Obj = doctors
            };
        }
    }
}
