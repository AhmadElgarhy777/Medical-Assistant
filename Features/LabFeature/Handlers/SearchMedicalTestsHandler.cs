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
    public class SearchMedicalTestsHandler : IRequestHandler<SearchMedicalTestsQuery, List<MedicalTestCardDTO>>
    {
        private readonly ApplicationDbContext _context;
        public SearchMedicalTestsHandler(ApplicationDbContext context) => _context = context;

        public async Task<List<MedicalTestCardDTO>> Handle(SearchMedicalTestsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.MedicalTests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
                query = query.Where(t => t.Name.Contains(request.Search));

            return await query
                .Select(t => new MedicalTestCardDTO(
                    t.ID, t.Name, t.Category, t.BasePrice, t.TurnaroundHours,
                    t.RequiresFasting, t.Description, t.PreparationInstructions))
                .ToListAsync(cancellationToken);
        }
    }
}