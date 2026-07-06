using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess;
using Features.LabFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;

namespace Features.LabFeature.Handlers
{
    public class GetAreasByGovernorateHandler : IRequestHandler<GetAreasByGovernorateQuery, List<AreaDTO>>
    {
        private readonly ApplicationDbContext _context;
        public GetAreasByGovernorateHandler(ApplicationDbContext context) => _context = context;

        public async Task<List<AreaDTO>> Handle(GetAreasByGovernorateQuery request, CancellationToken cancellationToken)
        {
            return await _context.Areas
                .Where(a => a.Governorate == request.Governorate)
                .Select(a => new AreaDTO(a.ID, a.Name, a.Governorate.ToString()))
                .ToListAsync(cancellationToken);
        }
    }
}
