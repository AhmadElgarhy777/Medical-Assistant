using MediatR;
using Models.DTOs;
using Features;

namespace Features.RadiologyFeature.Queries
{
    public record GetRadiologyDashboardQuery : IRequest<ResultResponse<object>>;
    public record GetRadiologyProfileQuery : IRequest<ResultResponse<object>>;
    public record GetRadiologyScansQuery(PaginationRequest Request) : IRequest<ResultResponse<PaginatedList<object>>>;
    public record GetRadiologyAppointmentsQuery(PaginationRequest Request, string? Status) : IRequest<ResultResponse<PaginatedList<object>>>;
    public record GetRadiologyAppointmentDetailsQuery(string Id) : IRequest<ResultResponse<object>>;
    public record GetRadiologyReportQuery(string Id) : IRequest<ResultResponse<object>>;
    public record GetRadiologyNotificationsQuery(PaginationRequest Request) : IRequest<ResultResponse<PaginatedList<object>>>;
    public record GetRadiologyAnalyticsQuery(string Type) : IRequest<ResultResponse<object>>; // Daily, Weekly, Monthly
    
    // Patient Queries
    public record GetRadiologyAreasQuery : IRequest<ResultResponse<object>>;
    public record GetRadiologyByAreaQuery(string AreaId, PaginationRequest Request) : IRequest<ResultResponse<PaginatedList<object>>>;
    public record GetRadiologyDetailsForPatientQuery(string Id) : IRequest<ResultResponse<object>>;
    public record GetPatientRadiologyBookingsQuery(PaginationRequest Request) : IRequest<ResultResponse<PaginatedList<object>>>;
    public record GetPatientRadiologyBookingDetailsQuery(string Id) : IRequest<ResultResponse<object>>;
}
