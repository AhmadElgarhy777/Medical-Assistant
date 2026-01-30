using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DTOs;

namespace Features.DoctorFeature.Queries
{
    public record GetDoctorAvailableSlotsQuery(string DoctorId) : IRequest<List<DoctorAvailableTimeDTO>>;
}
