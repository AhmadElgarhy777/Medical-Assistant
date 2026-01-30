using DataAccess;
using Features.DoctorFeature.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Features.DoctorFeature.Handlers
{
    public class GetDoctorAvailableSlotsHandler : IRequestHandler<GetDoctorAvailableSlotsQuery, List<DoctorAvailableTimeDTO>>
    {
        private readonly ApplicationDbContext _context;

        public GetDoctorAvailableSlotsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DoctorAvailableTimeDTO>> Handle(GetDoctorAvailableSlotsQuery request, CancellationToken cancellationToken)
        {
            // 1. سحب البيانات من الداتابيز للرام (Memory)
            var slotsFromDb = await _context.DoctorAvilableTimes
                .Where(x => x.DoctorId == request.DoctorId && x.IsBooked == false)
                .ToListAsync(cancellationToken);

            // 2. تحويل البيانات لـ DTO وتنسيق اليوم والوقت
            var result = slotsFromDb.Select(x => new DoctorAvailableTimeDTO
            {
                Id = x.ID,
                Day = x.Day.ToString(), // هتحول الرقم لاسم اليوم (Saturday)
                From = x.StartTime.ToString("HH:mm"),
                To = x.EndTime.ToString("HH:mm")
            }).ToList();

            return result;
        }
    }
}