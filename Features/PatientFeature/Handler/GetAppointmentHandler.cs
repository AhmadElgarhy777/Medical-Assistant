using AutoMapper;
using DataAccess.Repositry.IRepositry;
using Features.PatientFeature.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Handler
{
    public class GetAppointmentHandler : IRequestHandler<GetAppointmentsQuery, List<AppointmentDTO>>
    {
        private readonly IPatientRepositry patientRepositry;
        private readonly IAppointmentRepositry appointmentRepositry;
        private readonly IMapper mapper;

        public GetAppointmentHandler(IPatientRepositry patientRepositry,IAppointmentRepositry appointmentRepositry,IMapper mapper)
        {
            this.patientRepositry = patientRepositry;
            this.appointmentRepositry = appointmentRepositry;
            this.mapper = mapper;
        }
        public async Task<List<AppointmentDTO>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
        {
           var patientId = request.Id;
            var page = request.page;
            if(page<=0)page = 1;

            var app = appointmentRepositry.GetAll(includeProp: [e => e.Doctor, e => e.Patient], expression: e => e.PatientId == patientId);
               List<AppointmentDTO> appointmentDTOs = new List<AppointmentDTO>();
          
            if (app != null)
            {
                app= app.Skip((page-1)*5).Take(5);
                var appoinmentList= await app.ToListAsync(cancellationToken);
                foreach(var appo in appoinmentList)
                {
                    appointmentDTOs.Add(mapper.Map<AppointmentDTO>(appo));
                }
                
            }

         
            return appointmentDTOs;

        }
    }
}
