using AutoMapper;
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.PatientFeature.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Handler
{
    public class GetPresciptionItemHandler : IRequestHandler<GetPresciptionItemQuery, List<PresciptionItemDTO>>

    {
        private readonly IPresciptionItemRepositry itemRepositry;
        private readonly IMapper mapper;

        public GetPresciptionItemHandler(IPresciptionItemRepositry itemRepositry ,IMapper mapper)
        {
            this.itemRepositry= itemRepositry;
            this.mapper = mapper;
        }
        public async Task<List<PresciptionItemDTO>> Handle(GetPresciptionItemQuery request, CancellationToken cancellationToken)
        {
           var  presciptionId = request.presciptionId;
            var page = request.page;
            var spec = new PrescriptionItemSpecifcation(e => e.ID == presciptionId);

            var Items = itemRepositry.GetAll(spec);

                Items = Items?.Skip((page - 1) * 5).Take(5);
                var Item = Items?.ToList();
                   var ItemsDTOs=mapper.Map<List<PrescriptionItem>,List<PresciptionItemDTO>>(Item);
            return ItemsDTOs;
        }
    }
}
