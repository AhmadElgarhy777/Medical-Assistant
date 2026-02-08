using AutoMapper;
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using Features.AdminFeature.Queries;
using MediatR;
using Models.DTOs;
using Models.Enums;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Features.AdminFeature.Handlers
{
    public class GetPendingNurseHandler : IRequestHandler<GetPendingNurseQuery, ResultResponse<List<NurseRowDTO>>>
    {
        private readonly INuresRepositry nuresRepositry;
        private readonly IMapper mapper;

        public GetPendingNurseHandler(INuresRepositry nuresRepositry, IMapper mapper)
        {
            this.nuresRepositry = nuresRepositry;
            this.mapper = mapper;
        }
        public async Task<ResultResponse<List<NurseRowDTO>>> Handle(GetPendingNurseQuery request, CancellationToken cancellationToken)
        {
            var page = request.page;
            if (page < 1) page = 1;

            var spec = new NurseSpesfication(ConfrmationStatus.Pending);
            var nurse = await nuresRepositry.GetAll(spec).ToListAsync();

            if (nurse is not null && nurse.Count() > 0)
            {
                var nurseRows = mapper.Map<List<Nures>, List<NurseRowDTO>>(nurse);
                var nurseRowsPagnation = nurseRows.Skip((page - 1) * 5).Take(10).ToList();

                if (nurseRowsPagnation is not null && nurseRowsPagnation.Count() > 0)
                {
                    return new ResultResponse<List<NurseRowDTO>>
                    {
                        ISucsses = true,
                        Obj = nurseRowsPagnation
                    };
                }
            }

            return new ResultResponse<List<NurseRowDTO>>
            {
                ISucsses = false,
                Message = "Not Found Any Doctors"
            };
        }
    }
}

