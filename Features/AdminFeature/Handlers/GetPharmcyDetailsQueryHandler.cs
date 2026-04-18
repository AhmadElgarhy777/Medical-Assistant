using AutoMapper;
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.AdminFeature.Queries;
using MediatR;
using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AdminFeature.Handlers
{
    internal class GetPharmcyDetailsQueryHandler : IRequestHandler<GetPharmcyDetailsQuery, ResultResponse<PharmacyDetailsDTO>>
    {
        private readonly IPharmacyRepository pharmacyRepository;
        private readonly IMapper mapper;

        public GetPharmcyDetailsQueryHandler(IPharmacyRepository pharmacyRepository,IMapper mapper)
        {
            this.pharmacyRepository = pharmacyRepository;
            this.mapper = mapper;
        }
        public async Task<ResultResponse<PharmacyDetailsDTO>> Handle(GetPharmcyDetailsQuery request, CancellationToken cancellationToken)
        {
            var Id = request.pharmacyId;

            var pharmacy = await pharmacyRepository.GetPharmacyByIdAsync(Id);


            if (pharmacy is not null)
            {
                var pharmacyDetails = mapper.Map<Pharmacy, PharmacyDetailsDTO>(pharmacy);

                return new ResultResponse<PharmacyDetailsDTO>
                {
                    ISucsses = true,
                    Obj = pharmacyDetails
                };
            }
            return new ResultResponse<PharmacyDetailsDTO>
            {
                ISucsses = false,
                Message = "The pharmacy is not Found....!"
            };

        }
    }
}
