using AutoMapper;
using DataAccess.EntittySpecifcation;
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
    public class GetNurseHandler : IRequestHandler<SearchNuresQuery, List<SearchNurseResultDto>>
    {
        private readonly INuresRepositry nuresRepositry;
        private readonly IMapper mapper;

        public GetNurseHandler(INuresRepositry nuresRepositry,IMapper mapper)
        {
            this.nuresRepositry = nuresRepositry;
            this.mapper = mapper;
        }
        public async Task<List<SearchNurseResultDto>> Handle(SearchNuresQuery request, CancellationToken cancellationToken)
        {
            var page = request.page;
            if(request == null)
            {
                var Nullspec = new NurseSpesfication();
                var Nullnures = nuresRepositry.GetAll(Nullspec);
                var Allnures = Nullnures.Skip((page - 1) * 5).Take(5);

                var AllnureList = await Allnures.ToListAsync(cancellationToken);

                var NullResult = AllnureList.Select(n => new SearchNurseResultDto
                {
                    ID = n.ID,
                    UserName = n.UserName,
                    Gender = n.Gender,
                    Img = n.Img,
                    Age = DateTime.Now.Year - n.BD.Year,
                    Email = n.Email,
                    PricePerHours = n.PricePerHours,
                    NurseSpecialty = n.NurseSpecialty

                }).ToList();

                return NullResult;
            }
            var Search = new
            {
                name = request.Searching.Name,
                city = request.Searching.City,
                governorate = request.Searching.Governorate,
                rate = request.Searching.Rate,
                minrange=request.Searching.MinPrice,
                maxrange=request.Searching.MaxPrice,
                gender=request.Searching.Gender,
                nurseSpecialty=request.Searching.nurseSpecialty,
                nurseService=request.Searching.ServiceID,
            };
            if (page <= 0) page = 1;
            var spec = new NurseSpesfication();
            var nures = nuresRepositry.GetAll(spec);

            if (!string.IsNullOrWhiteSpace(Search.name))
                nures = nures.Where(e => e.FullName.Contains(Search.name));

            if (!string.IsNullOrWhiteSpace(Search.city))
                nures = nures.Where(e => e.City.Contains(Search.city));

            if (Search.governorate.HasValue)
                nures = nures.Where(e => e.Governorate.Equals(Search.governorate));

            if (Search.rate.HasValue)
                nures = nures.Where(e => e.RattingAverage>= (double)Search.rate);

            if (Search.nurseSpecialty.HasValue)
                nures = nures.Where(e => e.NurseSpecialty.Equals(Search.nurseSpecialty));

            if (Search.gender.HasValue)
                nures = nures.Where(e => e.Gender.Equals(Search.gender));

            if (Search.minrange.HasValue)
                nures = nures.Where(e => e.PricePerHours >= Search.minrange.Value);

            if (Search.maxrange.HasValue)
                nures = nures.Where(e => e.PricePerHours <= Search.maxrange.Value);
            if (!string.IsNullOrEmpty(Search.nurseService))
                nures = nures.Where(e => e.NurseServices.Any(ns => ns.ServiceId == Search.nurseService && !ns.IsDeleted));


            nures =nures.Skip((page-1)*5).Take(5);

            var nureList=await nures.ToListAsync(cancellationToken);

            var Result = nureList.Select(n => new SearchNurseResultDto
            {
                ID=n.ID,
                UserName=n.UserName,
                Gender=n.Gender,
                Img=n.Img,
                Age=DateTime.Now.Year - n.BD.Year,
                Email=n.Email,
                PricePerHours=n.PricePerHours,
                NurseSpecialty=n.NurseSpecialty

            }).ToList();
          



            return Result;




        }
    }
}
