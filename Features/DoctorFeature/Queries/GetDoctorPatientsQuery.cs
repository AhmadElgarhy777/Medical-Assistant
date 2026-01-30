using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Models.DTOs;
namespace Features.DoctorFeature.Queries
{
    // هنا بنقول إنه طلب (Query) وبيرجع قائمة (List) من بيانات المرضى
    public class GetDoctorPatientsQuery : IRequest<List<PatientDTO>>
    {
        public string DoctorId { get; set; }

        public GetDoctorPatientsQuery(string doctorId)
        {
            DoctorId = doctorId;
        }
    }
}
