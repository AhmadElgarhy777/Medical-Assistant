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
    public class GetLabsByAreaHandler : IRequestHandler<GetLabsByAreaQuery, List<LabListItemDTO>>
    {
        private readonly ApplicationDbContext _context;
        public GetLabsByAreaHandler(ApplicationDbContext context) => _context = context;

        public async Task<List<LabListItemDTO>> Handle(GetLabsByAreaQuery request, CancellationToken cancellationToken)
        {
            return await _context.Labs
                .Where(l => l.AreaId == request.AreaId && l.IsActive)
                .Include(l => l.Area)
                .Select(l => new LabListItemDTO(
                    l.ID, l.Name, l.Address, l.Area.Name,
                    l.Rating, l.ReviewsCount, l.ImageUrl,
                    l.WorkingHours, l.SupportsHomeCollection))
                .ToListAsync(cancellationToken);
        }
    }
}