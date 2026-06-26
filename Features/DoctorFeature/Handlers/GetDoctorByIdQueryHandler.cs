using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.DoctorFeature.Handlers
{
    public record GetDoctorByIdQueryHandler(string DocID,CancellationToken CancellationToken):IRequest<ResultResponse<Doctor>>;

    public class GetDoctorByIdQueryHandlerHandler : IRequestHandler<GetDoctorByIdQueryHandler, ResultResponse<Doctor>>
    {
        private readonly IDoctorRepositry doctorRepository;
        public GetDoctorByIdQueryHandlerHandler(IDoctorRepositry doctorRepository)
        {
            this.doctorRepository = doctorRepository;
        }
        public async Task<ResultResponse<Doctor>> Handle(GetDoctorByIdQueryHandler request, CancellationToken cancellationToken)
        {
            var spec = new DoctorSpecifcation(request.DocID);
            var doctor = await doctorRepository.GetOne(spec).FirstOrDefaultAsync(cancellationToken);
            if (doctor == null)
            {
                return new ResultResponse<Doctor>
                {
                    ISucsses = false,
                    Message = "Doctor not found.",
                };
            }
            return new ResultResponse<Doctor>
            {
                ISucsses = true,
                Obj = doctor
            };
        }
    }

}
