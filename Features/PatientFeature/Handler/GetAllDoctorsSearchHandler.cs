using AutoMapper;
using DataAccess.Repositry.IRepositry;
using Features.PatientFeature.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Handler
{
    public class GetAllDoctorsSearchHandler : IRequestHandler<GetAllDoctorsSearchQuery, List<DoctorDTO>>
    {
        private readonly IDoctorRepositry doctorRepositry;
        private readonly IMapper mapper;

        public GetAllDoctorsSearchHandler(IDoctorRepositry  doctorRepositry,IMapper mapper)
        {
            this.doctorRepositry = doctorRepositry;
            this.mapper = mapper;
        }
        public async Task<List<DoctorDTO>> Handle(GetAllDoctorsSearchQuery request, CancellationToken cancellationToken)
        {
            var page = request.page;
            var search = new
            {
                Name = request.searching.Name,
                City= request.searching.City,
                Rate= request.searching.Rate,
                Governorate= request.searching.Governorate,
                Specilzation= request.searching.Specilzation,

            };
            if(page<=0)page = 1;
            IQueryable<Doctor> doctors = doctorRepositry.GetAll(includeProp: [e => e.Specialization, e => e.Ratings]);

            if (!string.IsNullOrWhiteSpace(search.Name))
                doctors = doctors.Where(e => e.FullName.Contains(search.Name));

            if (!string.IsNullOrWhiteSpace(search.City))
                doctors = doctors.Where(e => e.City.Contains(search.City));

            if (!string.IsNullOrWhiteSpace(search.Specilzation))
                doctors = doctors.Where(e => e.Specialization.Name.Contains(search.Specilzation));

            if (search.Rate.HasValue)
                doctors = doctors.Where(e => e.RattingAverage >= (double)search.Rate.Value);

            if (search.Governorate.HasValue)
                doctors = doctors.Where(e => e.Governorate.Equals(search.Governorate));

          
            doctors =doctors.Skip((page-1) * 5 ).Take(5);
            var DoctorList = await doctors.ToListAsync(cancellationToken);
          
            var doctorDTOs = mapper.Map<List<Doctor>, List<DoctorDTO>>(DoctorList);

            return doctorDTOs.ToList(); 

                    
        }
    }
}
