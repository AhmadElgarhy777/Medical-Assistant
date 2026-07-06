using DataAccess.Repositry.IRepositry;
using Features.AdminFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AdminFeature.Handlers
{
    public class CreateNursingServiceCommandHandler : IRequestHandler<CreateNursingServiceCommand, ResultResponse<string>>
    {
        private readonly INursingServicesRepositry nursingServices;

        public CreateNursingServiceCommandHandler(INursingServicesRepositry nursingServices)
        {
            this.nursingServices = nursingServices;
        }

        public async Task<ResultResponse<string>> Handle(CreateNursingServiceCommand request, CancellationToken cancellationToken)
        {
            var existing = await nursingServices.GetTable()
                .AnyAsync(s => s.Name.ToLower() == request.Name.ToLower() && !s.IsDeleted, cancellationToken);

            if (existing)
                return new ResultResponse<string>
                {
                    ISucsses = false,
                    Message = "Service With This Name Already Exists"
                };

            var service = new NursingService
            {
                Name = request.Name,
                Description = request.Description
            };

            await nursingServices.GetTable().AddAsync(service, cancellationToken);
            await nursingServices.CommitAsync();

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "Service Created Successfully",
                Data = service.ID
            };
        }
    }
}
