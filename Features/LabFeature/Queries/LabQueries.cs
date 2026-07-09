using MediatR;
using Models.DTOs;
using Features;

namespace Features.LabFeature.Queries
{
    public record GetLabDashboardQuery : IRequest<ResultResponse<object>>;
    public record GetLabProfileQuery : IRequest<ResultResponse<object>>;
    public record GetLabTestsQuery(PaginationRequest Request) : IRequest<ResultResponse<PaginatedList<object>>>;
    public record GetLabBookingsQuery(PaginationRequest Request, string? Status) : IRequest<ResultResponse<PaginatedList<object>>>;
    public record GetLabBookingDetailsQuery(string Id) : IRequest<ResultResponse<object>>;
    public record GetLabResultQuery(string Id) : IRequest<ResultResponse<object>>;
    public record GetLabScheduleQuery : IRequest<ResultResponse<object>>;
    public record GetLabNotificationsQuery(PaginationRequest Request) : IRequest<ResultResponse<PaginatedList<object>>>;
    public record GetLabReportQuery(string Type) : IRequest<ResultResponse<object>>;
    
    // Home Collection Queries
    public record GetHomeCollectionRequestsQuery(PaginationRequest Request, string? Status) : IRequest<ResultResponse<PaginatedList<object>>>;
    public record GetHomeCollectionRequestDetailsQuery(string Id) : IRequest<ResultResponse<object>>;
    
    // Patient Queries
    public record GetLabAreasQuery : IRequest<ResultResponse<object>>;
    public record GetLabsByAreaQuery(string AreaId, PaginationRequest Request) : IRequest<ResultResponse<PaginatedList<object>>>;
    public record GetLabDetailsForPatientQuery(string Id) : IRequest<ResultResponse<object>>;
    public record SearchLabTestsQuery(string Query, PaginationRequest Request) : IRequest<ResultResponse<PaginatedList<object>>>;
    public record GetPatientLabBookingsQuery(PaginationRequest Request) : IRequest<ResultResponse<PaginatedList<object>>>;
    public record GetPatientLabBookingDetailsQuery(string Id) : IRequest<ResultResponse<object>>;
}
