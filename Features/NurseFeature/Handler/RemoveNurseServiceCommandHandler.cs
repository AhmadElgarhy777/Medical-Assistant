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
    public class RemoveNurseServiceCommandHandler : IRequestHandler<RemoveNurseServiceCommand, ResultResponse<string>>
    {
        private readonly INurseServicesRepositry nurseServicesRepositry;

        public RemoveNurseServiceCommandHandler(INurseServicesRepositry nurseServicesRepositry)
        {
            this.nurseServicesRepositry = nurseServicesRepositry;
        }

        public async Task<ResultResponse<string>> Handle(RemoveNurseServiceCommand request, CancellationToken cancellationToken)
        {
            var nurseService = await nurseServicesRepositry.GetTable()
                .FirstOrDefaultAsync(ns => ns.NurseId == request.NurseId
                                         && ns.ServiceId == request.ServiceId
                                         && !ns.IsDeleted, cancellationToken);

            if (nurseService is null)
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "This Service Is Not Added To This Nurse"
                };

            nurseServicesRepositry.GetTable().Remove(nurseService);
            await nurseServicesRepositry.CommitAsync();

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "Service Removed Successfully"
            };
        }
    }
}
