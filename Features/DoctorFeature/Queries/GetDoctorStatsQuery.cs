using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Models.DTOs;

namespace Features.DoctorFeature.Queries
{
    // بنقول للـ MediatR: "، خد الـ DoctorId وهات لي الـ DTO اللي فيه الأرقام"
    public record GetDoctorStatsQuery(string DoctorId) : IRequest<DoctorStatsDTO>;
}