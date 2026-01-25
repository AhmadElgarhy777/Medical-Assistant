using AutoMapper;
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.RegisterationFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.RegisterationFeature.Handelers
{
    public class GetAllSpecializationHandler : IRequestHandler<GetAllSpecializationQuery, ResultResponse<List<SpecializationDTO>>>
    {
        private readonly IMapper mapper;
        private readonly ISpecilizationRepositry specilizationRepositry;

        public GetAllSpecializationHandler(IMapper mapper,ISpecilizationRepositry specilizationRepositry)
        {
            this.mapper = mapper;
            this.specilizationRepositry = specilizationRepositry;
        }
        public async Task<ResultResponse<List<SpecializationDTO>>> Handle(GetAllSpecializationQuery request, CancellationToken cancellationToken)
        {
            var spec = new SpecilzationSpecifcation();
            var spcilizaions =await specilizationRepositry.GetAll(spec).ToListAsync(cancellationToken);

            var spcilizaionsDto = mapper.Map<List<Specialization>, List<SpecializationDTO>>(spcilizaions);
            if (spcilizaionsDto is not null)
            {
                return new ResultResponse<List<SpecializationDTO>>
                {
                    ISucsses = true,
                    Obj = spcilizaionsDto,
                };
            }

            return new ResultResponse<List<SpecializationDTO>>
            {
                ISucsses = false,
                Message="Invalid query"
            };
        }
    }
}
