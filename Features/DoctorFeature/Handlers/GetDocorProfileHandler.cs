using AutoMapper;
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.DoctorFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.DoctorFeature.Handlers
{
    public class GetDocorProfileHandler : IRequestHandler<GetDoctorProfileQuery, ResultResponse<DoctorDTO>>
    {
        private readonly IDoctorRepositry doctorRepositry;
        private readonly IMapper mapper;

        public GetDocorProfileHandler(IDoctorRepositry doctorRepositry,IMapper mapper)
        {
            this.doctorRepositry = doctorRepositry;
            this.mapper = mapper;
        }
        public async Task<ResultResponse<DoctorDTO>> Handle(GetDoctorProfileQuery request, CancellationToken cancellationToken)
        {
            var doctorId = request.doctorId;

            var spec = new DoctorSpecifcation(doctorId);
            var doctor = await doctorRepositry.GetOne(spec).FirstOrDefaultAsync();
            if (doctor is not null)
            {
                var doctorDto = mapper.Map<Doctor, DoctorDTO>(doctor);
                return new ResultResponse<DoctorDTO>
                {
                    ISucsses = true,
                    Obj = doctorDto
                };
            }

            return new ResultResponse<DoctorDTO>
            {
                ISucsses = false,
            };
        }
    }
}
