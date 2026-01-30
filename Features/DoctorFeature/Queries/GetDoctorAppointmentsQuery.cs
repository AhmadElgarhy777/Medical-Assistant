using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Models.DTOs;
namespace Features.DoctorFeature.Queries
{
    // بنبعت الـ DoctorId عشان نجيب حجوزاته هو بس
    public record GetDoctorAppointmentsQuery(string DoctorId) : IRequest<List<DoctorAppointmentsDTO>>;
}
