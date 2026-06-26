using DataAccess.Repositry.IRepositry;
using Features.PatientFeature.Static;
using MediatR;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientFeature.Handler
{
    public record GetNearestDoctorsQuery(
        string SpecializationId,
        double Latitude,
        double Longitude,
        double Radius,
        CancellationToken cancellationToken
    ) : IRequest<ResultResponse<List<NearestDoctorsDto>>>;


    public class GetNearestDoctorsQueryHandler
    : IRequestHandler<GetNearestDoctorsQuery,ResultResponse< List<NearestDoctorsDto>>>
    {
        private readonly IDoctorRepositry doctorRepositry;
        private readonly IMediator mediator;

        public GetNearestDoctorsQueryHandler(IDoctorRepositry doctorRepositry,IMediator mediator)
        {
            this.doctorRepositry = doctorRepositry;
            this.mediator = mediator;
        }

        public async Task<ResultResponse<List<NearestDoctorsDto>>> Handle(
            GetNearestDoctorsQuery request,
            CancellationToken cancellationToken)
        {
            var DoctorsResponse = await mediator.Send(new GetDoctorsBySpecifcationsQueryHandler(request.SpecializationId), cancellationToken);
            if(!DoctorsResponse.ISucsses) return new ResultResponse<List<NearestDoctorsDto>>
            {
                ISucsses = false,
                Message = DoctorsResponse.Message,
            };
            var doctors = DoctorsResponse.Obj;
            var DoctorsResult= doctors
                .Select(d =>
                {
                    var distance = CalcDistanceByHaversineFormala.CalculateDistance(
                        request.Latitude,
                        request.Longitude,
                        d.Latitude!.Value,
                        d.Longitude!.Value);

                    return new
                    {
                        Doctor = d,
                        Distance = distance
                    };
                })
                .Where(x => x.Distance <= request.Radius)
                .OrderBy(x => x.Distance)
                .Select(x => new NearestDoctorsDto
                {
                    DoctorId = x.Doctor.ID,
                    DoctorName = x.Doctor.FullName,
                    Specialization = x.Doctor.Specialization.Name,
                    Address = x.Doctor.Address,
                    City = x.Doctor.City,
                    Price = x.Doctor.Price,
                    Distance = Math.Round(x.Distance, 2),
                    DoctorRating = x.Doctor.RattingAverage
                })
                .ToList();

            if(DoctorsResult.Count == 0)
            {
                return new ResultResponse<List<NearestDoctorsDto>>
                {
                    ISucsses = false,
                    Message = "No doctors found within the specified radius.",
                };
            }
            return new ResultResponse<List<NearestDoctorsDto>>
            {
                ISucsses = true,
                Message = "Nearest doctors retrieved successfully.",
                Obj = DoctorsResult
            };
        }
    }

}
