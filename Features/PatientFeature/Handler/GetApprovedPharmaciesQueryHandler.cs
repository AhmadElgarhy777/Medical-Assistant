using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Handler
{
    public record GetApprovedPharmaciesQueryHandler():IRequest<ResultResponse< List<Pharmacy>>>;

    public class GetApprovedPharmaciesQueryHandlerHandler : IRequestHandler<GetApprovedPharmaciesQueryHandler, ResultResponse<List<Pharmacy>>>
    {
        private readonly IPharmacyReposities pharmacyReposities;

        public GetApprovedPharmaciesQueryHandlerHandler(IPharmacyReposities pharmacyReposities)
        {
            this.pharmacyReposities = pharmacyReposities;
        }
        public async Task<ResultResponse<List<Pharmacy>>> Handle(GetApprovedPharmaciesQueryHandler request, CancellationToken cancellationToken)
        {
            var spec = new PharmacySpecifcation(e=>e.Status==ConfrmationStatus.Approved
            && e.Latitude!=null
            &&e.Longitude!=null);
            var pharmacies = await pharmacyReposities.GetAll(spec).ToListAsync(cancellationToken);
            if(pharmacies==null || pharmacies.Count==0)
            {
                return new ResultResponse<List<Pharmacy>>
                {
                    ISucsses = false,
                    Message = "No approved pharmacies found.",
                };
            }
            return new ResultResponse<List<Pharmacy>>
            {
                ISucsses = true,
                Obj = pharmacies
            };


        }
    }

}
