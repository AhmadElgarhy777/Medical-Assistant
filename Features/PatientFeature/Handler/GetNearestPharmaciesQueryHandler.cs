using DataAccess.Repositry.IRepositry;
using Features.PatientFeature.Static;
using MediatR;
using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Handler
{
    public record GetNearestPharmaciesQuery(
    double Latitude,
    double Longitude,
    double Radius,
    CancellationToken cancellationToken
    ) : IRequest<ResultResponse<List<NearestPharmacyDto>>>;

    public class GetNearestPharmaciesQueryHandler
    : IRequestHandler<GetNearestPharmaciesQuery,ResultResponse<List<NearestPharmacyDto>>>
    {
        private readonly IPharmacyReposities pharmacyReposities;
        private readonly IMediator mediator;

        public GetNearestPharmaciesQueryHandler(IPharmacyReposities pharmacyReposities,IMediator mediator)
        {
            this.pharmacyReposities = pharmacyReposities;
            this.mediator = mediator;
        }
        public async Task<ResultResponse<List<NearestPharmacyDto>>> Handle(
            GetNearestPharmaciesQuery request,
            CancellationToken cancellationToken)
        {
            var pharmaciesResponse = await mediator.Send(new GetApprovedPharmaciesQueryHandler(), cancellationToken);
            if (!pharmaciesResponse.ISucsses)
            {
                return new ResultResponse<List<NearestPharmacyDto>>
                {
                    ISucsses = false,
                    Message = "No approved pharmacies found.",
                };
                }
            var pharmacies = pharmaciesResponse.Obj;

            var nearestPharmacies = pharmacies
                .Select(p =>
                {
                    var distance = CalcDistanceByHaversineFormala.CalculateDistance(
                        request.Latitude,
                        request.Longitude,
                        p.Latitude!.Value,
                        p.Longitude!.Value);

                    return new
                    {
                        Pharmacy = p,
                        Distance = distance
                    };
                })
                .Where(x => x.Distance <= request.Radius)
                .OrderBy(x => x.Distance)
                .Select(x => new NearestPharmacyDto
                {
                    PharmacyId = x.Pharmacy.ID,
                    PharmacyName = x.Pharmacy.Name,
                    Address = x.Pharmacy.Address,
                    Phone = x.Pharmacy.Phone,
                    Distance = Math.Round(x.Distance, 2)
                })
                .ToList();

            if (!nearestPharmacies.Any())
            {
                return new ResultResponse<List<NearestPharmacyDto>>
                {
                    ISucsses = false,
                    Message = "No pharmacies found within the specified radius.",
                };
            }
            return new ResultResponse<List<NearestPharmacyDto>>
            {
                ISucsses = true,
                Obj = nearestPharmacies
            };
        }
    }

}
