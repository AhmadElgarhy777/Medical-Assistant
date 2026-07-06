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
    public class GetLabDetailsHandler : IRequestHandler<GetLabDetailsQuery, LabDetailsDTO?>
    {
        private readonly ApplicationDbContext _context;
        public GetLabDetailsHandler(ApplicationDbContext context) => _context = context;

        public async Task<LabDetailsDTO?> Handle(GetLabDetailsQuery request, CancellationToken cancellationToken)
        {
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.ID == request.LabId, cancellationToken);
            if (lab == null) return null;

            var offersQuery = _context.LabTestOffers
                .Where(o => o.LabId == request.LabId && o.IsAvailable)
                .Include(o => o.MedicalTest)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
                offersQuery = offersQuery.Where(o => o.MedicalTest.Name.Contains(request.Search));

            var tests = await offersQuery
                .Select(o => new MedicalTestCardDTO(
                    o.MedicalTest.ID, o.MedicalTest.Name, o.MedicalTest.Category,
                    o.Price ?? o.MedicalTest.BasePrice, o.MedicalTest.TurnaroundHours,
                    o.MedicalTest.RequiresFasting, o.MedicalTest.Description, o.MedicalTest.PreparationInstructions))
                .ToListAsync(cancellationToken);

            return new LabDetailsDTO(lab.ID, lab.Name, lab.Address, lab.Phone,
                lab.Rating, lab.ReviewsCount, lab.WorkingHours, tests);
        }
    }
}