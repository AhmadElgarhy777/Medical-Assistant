using AutoMapper;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.PatientFeature.Query;
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
    public class GetPresciptionItemHandler : IRequestHandler<GetPresciptionItemQuery, List<PresciptionItemDTO>>

    {
        private readonly IPresciptionRepositry presciptionRepositry;
        private readonly IMapper mapper;

        public GetPresciptionItemHandler(IPresciptionRepositry presciptionRepositry ,IMapper mapper)
        {
            this.presciptionRepositry = presciptionRepositry;
            this.mapper = mapper;
        }
        public async Task<List<PresciptionItemDTO>> Handle(GetPresciptionItemQuery request, CancellationToken cancellationToken)
        {
           var  presciptionId = request.presciptionId;
            var page = request.page;

            var presciption = await presciptionRepositry.GetOneAsync(expression: e => e.PresciptionId == presciptionId, includeProp: [e => e.items]);
            List<PresciptionItemDTO> ItemsDTOs = new List<PresciptionItemDTO>();
            if (presciption != null)
            {
                var Items = presciption.items.Where(e => e.PresciptionId == presciption.PresciptionId);
                Items = Items.Skip((page - 1) * 5).Take(5);
                Items = Items.ToList();
                foreach (var item in Items)
                {
                    ItemsDTOs.Add(mapper.Map<PresciptionItemDTO>(item));
                }


            }
            return ItemsDTOs;
        }
    }
}
