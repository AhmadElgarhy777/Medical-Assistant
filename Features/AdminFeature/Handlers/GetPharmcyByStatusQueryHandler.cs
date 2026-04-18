using AutoMapper;
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.AdminFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AdminFeature.Handlers
{
    public class GetPharmcyByStatusQueryHandler : IRequestHandler<GetPharmcyByStatusQuery, ResultResponse<List<PharmacyRowDTO>>>
    {
        private readonly IPharmacyRepository pharmacyRepository;
        private readonly IMapper mapper;

        public GetPharmcyByStatusQueryHandler(IPharmacyRepository pharmacyRepository,IMapper mapper)
        {
            this.pharmacyRepository = pharmacyRepository;
            this.mapper = mapper;
        }
        public async Task<ResultResponse<List<PharmacyRowDTO>>> Handle(GetPharmcyByStatusQuery request, CancellationToken cancellationToken)
        {
            var page = request.page;
            var status = request.status;    
            if (page < 1) page = 1;


            List<Pharmacy> pharmacies = new List<Pharmacy>();
            if (status.Equals(ConfrmationStatus.Approved))
            {
                 pharmacies =await pharmacyRepository.GetAllPharmacyByConfirmationStatusAsync(ConfrmationStatus.Approved).ToListAsync();
            }
            
            else if (status.Equals(ConfrmationStatus.Pending))
            {
                 pharmacies = await pharmacyRepository.GetAllPharmacyByConfirmationStatusAsync(ConfrmationStatus.Pending).ToListAsync();
            }

             var pharmacyRow = mapper.Map<List<Pharmacy>, List<PharmacyRowDTO>>(pharmacies);
             var pharmacyRowPagnation = pharmacyRow.Skip((page - 1) * 5).Take(10).ToList();

                if (pharmacyRowPagnation is not null && pharmacyRowPagnation.Count() > 0)
                {
                    return new ResultResponse<List<PharmacyRowDTO>>
                    {
                        ISucsses = true,
                        Obj = pharmacyRowPagnation
                    };
                }
            return new ResultResponse<List<PharmacyRowDTO>>
            {
                ISucsses = false,
                Message = "Not Found Any pharmacy"
            };
        }
    }
}
