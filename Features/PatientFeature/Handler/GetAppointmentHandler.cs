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
    public class GetAppointmentHandler : IRequestHandler<GetAppointmentsQuery, ResultResponse<List<AppointmentDTO>>>
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
        public async Task<ResultResponse<List<AppointmentDTO>>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var patientId = request.Id;
            var page = request.page;
            if(page<=0)page = 1;

            var spec = new AppointmentSpescifcation(e => e.PatientId == patientId);
            var app = appointmentRepositry.GetAll(spec);
           
            var pagnation = app.Skip((page-1)*5).Take(5);
            var appoinmentList= await pagnation.ToListAsync(cancellationToken);
            if(appoinmentList is not null)
            {
                var appointmentDTOs=mapper.Map<IEnumerable<Appointment>,List<AppointmentDTO>>(appoinmentList);

                return new ResultResponse<List<AppointmentDTO>>
                {
                    ISucsses=true,
                    Obj= appointmentDTOs
                };
            }
            
            return new ResultResponse<List<AppointmentDTO>>
            {
                ISucsses = false,
                Message="not Found Any Apoinment"
            };

        }
    }
}
