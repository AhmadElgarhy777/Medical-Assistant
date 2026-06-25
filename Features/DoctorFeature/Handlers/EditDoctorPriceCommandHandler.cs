using DataAccess.EntittySpecifcation;
using DataAccess.Repositry.IRepositry;
using Features.DoctorFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.DoctorFeature.Handlers
{
    public class EditDoctorPriceCommandHandler : IRequestHandler<EditDoctorPriceCommand, ResultResponse<bool>>
    {
        private readonly IDoctorRepositry doctorRepositry;

        public EditDoctorPriceCommandHandler(IDoctorRepositry doctorRepositry)
        {
            this.doctorRepositry = doctorRepositry;
        }
        public async Task<ResultResponse<bool>> Handle(EditDoctorPriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var spec = new DoctorSpecifcation(request.DocId);
                var doctor = await doctorRepositry.GetOne(spec).FirstOrDefaultAsync();
                if (doctor == null) return new ResultResponse<bool> { ISucsses = false, Message = "the doctor not found" };
                doctor.Price = request.price;

                doctorRepositry.Edit(doctor);
                await doctorRepositry.CommitAsync();

                return new ResultResponse<bool> { ISucsses = true };
            }
            catch(Exception ex) 
            {
                return new ResultResponse<bool> { ISucsses = false, Message = ex.Message };
            }
           

        }
    }
}
