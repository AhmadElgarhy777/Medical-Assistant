using AutoMapper;
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
    public class GetNurseHandler : IRequestHandler<SearchNuresQuery, List<NurseDTO>>
    {
        private readonly INuresRepositry nuresRepositry;
        private readonly IMapper mapper;

        public GetNurseHandler(INuresRepositry nuresRepositry,IMapper mapper)
        {
            this.nuresRepositry = nuresRepositry;
            this.mapper = mapper;
        }
        public async Task<List<NurseDTO>> Handle(SearchNuresQuery request, CancellationToken cancellationToken)
        {
            var page = request.page;
            var Search = new
            {
                name = request.Searching.Name,
                city = request.Searching.City,
                governorate = request.Searching.Governorate,
                rate = request.Searching.Rate,
                minrange=request.Searching.MinPrice,
                maxrange=request.Searching.MaxPrice,
                gender=request.Searching.Gender,
            };
            if (page <= 0) page = 1;
            IQueryable<Nures> nures = nuresRepositry.GetAll(includeProp: [e=>e.Ratings]);
            if (!string.IsNullOrWhiteSpace(Search.name))
                nures = nures.Where(e => e.FullName.Contains(Search.name));

            if (!string.IsNullOrWhiteSpace(Search.city))
                nures = nures.Where(e => e.City.Contains(Search.city));

            if (Search.governorate.HasValue)
                nures = nures.Where(e => e.Governorate.Equals(Search.governorate));

            if (Search.rate.HasValue)
                nures = nures.Where(e => e.RattingAverage>= (double)Search.rate);


            if (Search.gender.HasValue)
                nures = nures.Where(e => e.Gender.Equals(Search.gender));

            if (Search.minrange.HasValue)
                nures = nures.Where(e => e.PricePerDay >= Search.minrange.Value);

            if (Search.maxrange.HasValue)
                nures = nures.Where(e => e.PricePerDay <= Search.maxrange.Value);

            nures =nures.Skip((page-1)*5).Take(5);

            List<Nures> nureList=await nures.ToListAsync(cancellationToken);
           
               var nurseListDTO=mapper.Map<List<Nures>,List<NurseDTO>>(nureList);
            
            return nurseListDTO;




        }
    }
}
