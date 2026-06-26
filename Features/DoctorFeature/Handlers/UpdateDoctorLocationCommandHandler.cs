using DataAccess.Repositry.IRepositry;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.DoctorFeature.Handlers
{
    public record UpdateDoctorLocationCommand(
     string DoctorId,
     double Latitude,
     double Longitude,
     CancellationToken cancellationToken
 ) : IRequest<ResultResponse<bool>>;

    public class UpdateDoctorLocationCommandHandler
    : IRequestHandler<UpdateDoctorLocationCommand, ResultResponse<bool>>
    {
        private readonly IMediator mediator;
        private readonly IDoctorRepositry doctorRepositry;

        public UpdateDoctorLocationCommandHandler(IMediator mediator ,IDoctorRepositry doctorRepositry)
        {
            this.mediator = mediator;
            this.doctorRepositry = doctorRepositry;
        }

        public async Task<ResultResponse<bool>> Handle(
            UpdateDoctorLocationCommand request,
            CancellationToken cancellationToken)
        {
            var DoctorResponse = await mediator.Send(new GetDoctorByIdQueryHandler(request.DoctorId,cancellationToken), cancellationToken);
            if (!DoctorResponse.ISucsses)
            {
                return new ResultResponse<bool>
                {
                    ISucsses = false,
                    Message = DoctorResponse.Message,
                    Obj = false,
                };
            }
            var doctor = DoctorResponse.Obj;

            doctor.Latitude = request.Latitude;
            doctor.Longitude = request.Longitude;

            await doctorRepositry.CommitAsync();

            return new ResultResponse<bool>
            {
                ISucsses = true,
                Message = "Doctor location updated successfully.",
                Obj = true,
            };
        }
    }
}
