using MediatR;
using Models.DTOs;
using Features;

namespace Features.RadiologyFeature.Queries
{
    public record GetRadiologyScheduleQuery : IRequest<ResultResponse<object>>;
}
