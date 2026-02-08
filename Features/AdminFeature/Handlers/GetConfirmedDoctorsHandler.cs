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
    internal class GetConfirmedDoctorsHandler : IRequestHandler<GetConfirmedDoctorsQuery, ResultResponse<List<DoctorRowDTO>>>
    {
        private readonly IDoctorRepositry doctorRepositry;
        private readonly IMapper mapper;

        public GetConfirmedDoctorsHandler(IDoctorRepositry doctorRepositry, IMapper mapper)
        {
            this.doctorRepositry = doctorRepositry;
            this.mapper = mapper;
        }
        public async Task<ResultResponse<List<DoctorRowDTO>>> Handle(GetConfirmedDoctorsQuery request, CancellationToken cancellationToken)
        {
            var page = request.page;
            if (page < 1) page = 1;

            var spec = new DoctorSpecifcation(ConfrmationStatus.Approved);
            var doctors = await doctorRepositry.GetAll(spec).ToListAsync();

            if(doctors is not null&&doctors.Count()>0)
            {
                var doctorsRows = mapper.Map<List<Doctor>, List<DoctorRowDTO>>(doctors);
                var doctorsRowsPagnation = doctorsRows.Skip((page - 1) * 5).Take(10).ToList();

                if (doctorsRowsPagnation is not null&&doctorsRowsPagnation.Count()>0)
                {
                    return new ResultResponse<List<DoctorRowDTO>>
                    {
                        ISucsses = true,
                        Obj = doctorsRowsPagnation
                    };
                }
            }
            
            return new ResultResponse<List<DoctorRowDTO>>
            {
                ISucsses = false,
                Message = "Not Found Any Doctors"
            };

        }
    }


}
