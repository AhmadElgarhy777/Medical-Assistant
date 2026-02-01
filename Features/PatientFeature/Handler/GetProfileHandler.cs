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
    public class GetProfileHandler : IRequestHandler<GetPatientProfileQuery, ResultResponse<PatientPeofileDTO>>
    {
        private readonly IPatientRepositry patientRepositry;
        private readonly IMapper mapper;

        public GetProfileHandler(IPatientRepositry patientRepositry,IMapper mapper)
        {
            this.patientRepositry = patientRepositry;
            this.mapper = mapper;
        }
        public async Task<ResultResponse<PatientPeofileDTO>> Handle(GetPatientProfileQuery request, CancellationToken cancellationToken)
        {
            var patientId = request.patientId;

            var spec = new PatientSpecifcation(patientId);
            var patient = await patientRepositry.GetOne(spec).FirstOrDefaultAsync();
            if (patient is not null)
            {
                var patientDto = mapper.Map<Patient, PatientPeofileDTO>(patient);
                return new ResultResponse<PatientPeofileDTO>
                {
                    ISucsses = true,
                    Obj = patientDto
                };
            }

            return new ResultResponse<PatientPeofileDTO>
            {
                ISucsses = false,
            };
        }
    }
}
