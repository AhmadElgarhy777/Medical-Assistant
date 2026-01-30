using MediatR;
using Models.DTOs;
using System.Collections.Generic;

namespace Features.DoctorFeature.Queries
{
    public record GetDoctorHistoryQuery(string DoctorId) : IRequest<List<DoctorAppointmentsDTO>>;
}
