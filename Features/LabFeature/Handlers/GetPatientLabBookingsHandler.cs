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
    public class GetPatientLabBookingsHandler : IRequestHandler<GetPatientLabBookingsQuery, List<LabBookingSummaryDTO>>
    {
        private readonly ApplicationDbContext _context;
        public GetPatientLabBookingsHandler(ApplicationDbContext context) => _context = context;

        public async Task<List<LabBookingSummaryDTO>> Handle(GetPatientLabBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _context.LabBookings
                .Where(b => b.PatientId == request.PatientId)
                .Include(b => b.Lab)
                .Include(b => b.RadiologyCenter)
                .Include(b => b.Items)
                .OrderByDescending(b => b.ScheduledDate)
                .ToListAsync(cancellationToken);

            return bookings.Select(b => new LabBookingSummaryDTO(
                b.ID,
                b.ServiceType.ToString(),
                b.VisitType.ToString(),
                b.VisitType == Models.Enums.VisitTypeEnum.HomeCollection
                    ? "زيارة منزلية"
                    : (b.Lab != null ? b.Lab.Name : b.RadiologyCenter?.Name ?? ""),
                b.ScheduledDate, b.ScheduledTimeSlot, b.Status.ToString(),
                b.TotalPrice, b.Items.Count
            )).ToList();
        }
    }
}