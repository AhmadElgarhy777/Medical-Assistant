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
    public record NursingServiceDto(string ID, string Name, string? Description);

    public record GetAllServicesQuery : IRequest<ResultResponse<List<NursingServiceDto>>>;

    public class GetAllServicesQueryHandler
        : IRequestHandler<GetAllServicesQuery, ResultResponse<List<NursingServiceDto>>>
    {
        private readonly INursingServicesRepositry nursingServices;

        public GetAllServicesQueryHandler(INursingServicesRepositry nursingServices)
        {
            this.nursingServices = nursingServices;
        }

        public async Task<ResultResponse<List<NursingServiceDto>>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            var services = await nursingServices.GetTable()
                .Where(s => !s.IsDeleted)
                .Select(s => new NursingServiceDto(s.ID, s.Name, s.Description))
                .ToListAsync(cancellationToken);

            return new ResultResponse<List<NursingServiceDto>>
            {
                ISucsses = true,
                Message = "Services Retrieved Successfully",
                Obj = services
            };
        }
    }
}
