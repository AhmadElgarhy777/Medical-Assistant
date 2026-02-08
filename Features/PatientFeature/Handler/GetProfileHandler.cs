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
        private readonly IPatientPhoneRepositry patientPhoneRepositry;

        public GetProfileHandler(IPatientRepositry patientRepositry,IMapper mapper,IPatientPhoneRepositry patientPhoneRepositry)
        {
            this.patientRepositry = patientRepositry;
            this.mapper = mapper;
            this.patientPhoneRepositry = patientPhoneRepositry;
        }
        public async Task<ResultResponse<PatientPeofileDTO>> Handle(GetPatientProfileQuery request, CancellationToken cancellationToken)
        {
            var patientId = request.patientId;

            var spec = new PatientSpecifcation(patientId);
            var patient = await patientRepositry.GetOne(spec).FirstOrDefaultAsync();

            var phonesspec = new PatientPhonesSpecifcation(patientId);
            var patientPhones=patientPhoneRepositry.GetAll(phonesspec).ToList();

            var patientPhonesDto = mapper.Map<List<PatientPhone>,List<PatientPhonesDTO>>(patientPhones);


            if (patient is not null)
            {
                var patientDto = mapper.Map<Patient, PatientPeofileDTO>(patient);
                patientDto.Phones= patientPhonesDto;
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
