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
    public class GetRadiologyCenterDetailsHandler : IRequestHandler<GetRadiologyCenterDetailsQuery, LabDetailsDTO?>
    {
        private readonly ApplicationDbContext _context;
        public GetRadiologyCenterDetailsHandler(ApplicationDbContext context) => _context = context;

        public async Task<LabDetailsDTO?> Handle(GetRadiologyCenterDetailsQuery request, CancellationToken cancellationToken)
        {
            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(c => c.ID == request.CenterId, cancellationToken);
            if (center == null) return null;

            var offersQuery = _context.RadiologyCenterScans
                .Where(o => o.RadiologyCenterId == request.CenterId && o.IsAvailable)
                .Include(o => o.RadiologyScan)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
                offersQuery = offersQuery.Where(o => o.RadiologyScan.Name.Contains(request.Search));

            var scans = await offersQuery
                .Select(o => new MedicalTestCardDTO(
                    o.RadiologyScan.ID, o.RadiologyScan.Name, o.RadiologyScan.Category,
                    o.Price ?? o.RadiologyScan.BasePrice, o.RadiologyScan.TurnaroundHours,
                    false, o.RadiologyScan.Description, o.RadiologyScan.PreparationInstructions))
                .ToListAsync(cancellationToken);

            return new LabDetailsDTO(center.ID, center.Name, center.Address, center.Phone,
                center.Rating, center.ReviewsCount, center.WorkingHours, scans);
        }
    }
}