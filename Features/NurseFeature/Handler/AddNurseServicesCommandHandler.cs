using DataAccess;
using DataAccess.EntittySpecifcation;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.NurseFeature.Command;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.NurseFeature.Handler
{

    public class AddNurseServicesCommandHandler : IRequestHandler<AddNurseServicesCommand, ResultResponse<string>>
    {
        private readonly INursingServicesRepositry nursingServices;
        private readonly INurseServicesRepositry nurseServicesRepositry;
        private readonly INuresRepositry nuresRepositry;

        public AddNurseServicesCommandHandler(INursingServicesRepositry nursingServices
            , INurseServicesRepositry nurseServicesRepositry,
            INuresRepositry nuresRepositry
            )
        {
            this.nursingServices = nursingServices;
            this.nurseServicesRepositry = nurseServicesRepositry;
            this.nuresRepositry = nuresRepositry;
        }

        public async Task<ResultResponse<string>> Handle(AddNurseServicesCommand request, CancellationToken cancellationToken)
        {
            // 1. تأكد إن الممرض موجود
            var spec = new NurseSpesfication(request.NurseId);
            var nurse = await nuresRepositry.GetOne(spec).FirstOrDefaultAsync(cancellationToken);
            if (nurse is null)
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Nurse Not Found"
                };

            if (request.ServiceIds is null || !request.ServiceIds.Any())
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "No Services Provided"
                };

            var distinctServiceIds = request.ServiceIds.Distinct().ToList();

            // 2. تأكد إن كل الـ Services دي موجودة فعلاً
            var existingServices = await nursingServices
                .GetTable().Where(s => distinctServiceIds.Contains(s.ID) && !s.IsDeleted).ToListAsync(cancellationToken);

            var existingServiceIds = existingServices.Select(s => s.ID).ToList();
            var notFoundIds = distinctServiceIds.Except(existingServiceIds).ToList();
            if (notFoundIds.Any())
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = $"These Services Not Found: {string.Join(", ", notFoundIds)}"
                };

            // 3. تأكد إن الممرض لسه مضايفهاش قبل كده
            var alreadyAdded = await nurseServicesRepositry.GetTable()
                .Where(ns => ns.NurseId == request.NurseId
                               && distinctServiceIds.Contains(ns.ServiceId)
                               && !ns.IsDeleted).ToListAsync(cancellationToken);


            var duplicateIds = alreadyAdded.Select(x => x.ServiceId).ToList();
            if (duplicateIds.Any())
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = $"These Services Already Added: {string.Join(", ", duplicateIds)}"
                };

            // 4. ضيف كل الـ NurseService
            foreach (var serviceId in distinctServiceIds)
            {
                await nurseServicesRepositry.GetTable().AddAsync(new NurseService
                {
                    NurseId = request.NurseId,
                    ServiceId = serviceId
                });
            }
            await nurseServicesRepositry.CommitAsync();

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "Services Added Successfully"
            };
        }
    }
}
