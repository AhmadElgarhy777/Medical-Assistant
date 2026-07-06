using DataAccess.Repositry.IRepositry;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.NurseFeature.Handler
{
    public class NurseServiceDto
    {
        public string ID { get; set; } = null!;
        public string Name { get; set; } = null!;

    }

    public record GetAllNursingServicesQuery(string NurseID) : IRequest<ResultResponse<List<NurseServiceDto>>>;

    public class GetAllNursingServicesQueryHandler
        : IRequestHandler<GetAllNursingServicesQuery, ResultResponse<List<NurseServiceDto>>>
    {
        private readonly INursingServicesRepositry nursingServices;
        private readonly INurseServicesRepositry nurseServicesRepositry;

        public GetAllNursingServicesQueryHandler(INursingServicesRepositry nursingServices,INurseServicesRepositry nurseServicesRepositry)
        {
            this.nursingServices = nursingServices;
            this.nurseServicesRepositry = nurseServicesRepositry;
        }

        public async Task<ResultResponse<List<NurseServiceDto>>> Handle(GetAllNursingServicesQuery request, CancellationToken cancellationToken)
        {
            var services = await nurseServicesRepositry.GetTable()
                .Where(s => !s.IsDeleted&&s.NurseId==request.NurseID)
                .Select(s => new NurseServiceDto { ID=s.ServiceId,Name=s.Service.Name})
                .ToListAsync(cancellationToken);

            if (!services.Any())
            {
                return new ResultResponse<List<NurseServiceDto>>
                {
                    ISucsses = false,
                    Message = "No Service For You",
                };
            }

            return new ResultResponse<List<NurseServiceDto>>
            {
                ISucsses = true,
                Message = "Services Retrieved Successfully",
                Obj = services
            };
        }
    }
}
