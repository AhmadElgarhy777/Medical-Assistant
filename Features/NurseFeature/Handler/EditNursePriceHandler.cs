using DataAccess.EntittySpecifcation;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.NurseFeature.Command;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.NurseFeature.Handler
{
    public class EditNursePriceHandler : IRequestHandler<EditNursePriceCommand, ResultResponse<bool>>
    {
        private readonly INuresRepositry nuresRepositry;

        public EditNursePriceHandler(INuresRepositry nuresRepositry)
        {
            this.nuresRepositry = nuresRepositry;
        }
        public async Task<ResultResponse<bool>> Handle(EditNursePriceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var spec = new NurseSpesfication(request.NurseId);
                var nurse = await nuresRepositry.GetOne(spec).FirstOrDefaultAsync();
                if (nurse == null) return new ResultResponse<bool> { ISucsses = false, Message = "the nurse not found" };
                nurse.PricePerHours = request.price;

                nuresRepositry.Edit(nurse);
                await nuresRepositry.CommitAsync();

                return new ResultResponse<bool> { ISucsses = true };
            }
            catch (Exception ex)
            {
                return new ResultResponse<bool> { ISucsses = false, Message = ex.Message };
            }
        }
    }
}
