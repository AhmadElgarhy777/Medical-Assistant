using AutoMapper;
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using Features.AdminFeature.Queries;
using MediatR;
using Models.DTOs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Features.AdminFeature.Handlers
{
    public class GetNurseDetailsHandler : IRequestHandler<GetNurseDetailsQuery, ResultResponse<NurseDetailseDTO>>
    {
        private readonly INuresRepositry nuresRepositry;
        private readonly IMapper mapper;

        public GetNurseDetailsHandler(INuresRepositry nuresRepositry, IMapper mapper)
        {
            this.nuresRepositry = nuresRepositry;
            this.mapper = mapper;
        }
        public async Task<ResultResponse<NurseDetailseDTO>> Handle(GetNurseDetailsQuery request, CancellationToken cancellationToken)
        {
            var Id = request.nurseId;

            var spec = new NurseSpesfication(Id);
            var nurse = await nuresRepositry.GetOne(spec).FirstOrDefaultAsync();

            if (nurse is not null)
            {
                var nurseDetails = mapper.Map<Nures, NurseDetailseDTO>(nurse);

                return new ResultResponse<NurseDetailseDTO>
                {
                    ISucsses = true,
                    Obj = nurseDetails
                };
            }
            return new ResultResponse<NurseDetailseDTO>
            {
                ISucsses = false,
                Message = "The user is not Found....!"
            };

        }
    }
}

