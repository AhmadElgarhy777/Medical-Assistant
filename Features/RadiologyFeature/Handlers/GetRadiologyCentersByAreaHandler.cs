using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess;
using Features.RadiologyFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;

namespace Features.RadiologyFeature.Handlers
{
    public class GetRadiologyCentersByAreaHandler : IRequestHandler<GetRadiologyCentersByAreaQuery, List<LabListItemDTO>>
    {
        private readonly ApplicationDbContext _context;
        public GetRadiologyCentersByAreaHandler(ApplicationDbContext context) => _context = context;

        public async Task<List<LabListItemDTO>> Handle(GetRadiologyCentersByAreaQuery request, CancellationToken cancellationToken)
        {
            return await _context.RadiologyCenters
                .Where(c => c.AreaId == request.AreaId && c.IsActive)
                .Include(c => c.Area)
                .Select(c => new LabListItemDTO(
                    c.ID, c.Name, c.Address, c.Area.Name,
                    c.Rating, c.ReviewsCount, c.ImageUrl, c.WorkingHours, false))
                .ToListAsync(cancellationToken);
        }
    }
}