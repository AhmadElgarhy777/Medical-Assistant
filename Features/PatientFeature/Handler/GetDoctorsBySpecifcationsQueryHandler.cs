using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Handler
{
    public record GetDoctorsBySpecifcationsQueryHandler(string spcilizationId):IRequest<ResultResponse<List<Doctor>>>;

    public class GetDoctorsBySpecifcationsQueryHandlerHandler : IRequestHandler<GetDoctorsBySpecifcationsQueryHandler,ResultResponse<List<Doctor>>>
    {
        private readonly IDoctorRepositry doctorRepositry;

        public GetDoctorsBySpecifcationsQueryHandlerHandler(IDoctorRepositry doctorRepositry)
        {
            this.doctorRepositry = doctorRepositry;
        }
        public async Task<ResultResponse<List<Doctor>>> Handle(GetDoctorsBySpecifcationsQueryHandler request, CancellationToken cancellationToken)
        {
            var spec= new DoctorSpecifcation(e=>e.SpecializationId==request.spcilizationId
            &&e.IsDeleted==false
            &&e.Status==ConfrmationStatus.Approved
            &&e.Latitude!=null
            &&e.Longitude!= null
            );
            var doctors = await doctorRepositry.GetAll(spec).ToListAsync(cancellationToken);
            if(doctors==null || doctors.Count==0)
            {
                return new ResultResponse<List<Doctor>>
                {
                    ISucsses = false,
                    Message = "No doctors found for the specified specialization.",
                };
            }
            return new ResultResponse<List<Doctor>>
            {
                ISucsses = true,
                Obj=doctors
            };
        }
    }

}
