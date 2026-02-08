using AutoMapper;
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using Features.AdminFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AdminFeature.Handlers
{
    public class GetDoctorDetailsHandler : IRequestHandler<GetDoctorDetailsQuery, ResultResponse<DoctorDetailsDTO>>
    {
        private readonly IDoctorRepositry doctorRepositry;
        private readonly IMapper mapper;

        public GetDoctorDetailsHandler(IDoctorRepositry doctorRepositry,IMapper mapper)
        {
            this.doctorRepositry = doctorRepositry;
            this.mapper = mapper;
        }
        public async Task<ResultResponse<DoctorDetailsDTO>> Handle(GetDoctorDetailsQuery request, CancellationToken cancellationToken)
        {
            var Id = request.doctorId;

            var spec = new DoctorSpecifcation(Id);
            var doctor = await doctorRepositry.GetOne(spec).FirstOrDefaultAsync();

            if(doctor is not null)
            {
                var doctorDetails = mapper.Map<Doctor, DoctorDetailsDTO>(doctor);

                return new ResultResponse<DoctorDetailsDTO>
                {
                    ISucsses = true,
                    Obj=doctorDetails
                };
            }
            return new ResultResponse<DoctorDetailsDTO>
            {
                ISucsses = false,
                Message = "The user is not Found....!"
            };

        }
    }
}
