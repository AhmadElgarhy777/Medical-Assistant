using DataAccess;
using Features.DoctorFeature.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.DoctorFeature.Handlers
{
    public class CreateDoctorSlotsHandler : IRequestHandler<CreateDoctorSlotsCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public CreateDoctorSlotsHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateDoctorSlotsCommand request, CancellationToken cancellationToken)
        {
            var startTime = TimeOnly.Parse(request.FromTime);
            var endTime = TimeOnly.Parse(request.ToTime);
            var slots = new List<DoctorAvilableTime>();

            // هنقسم الفترة لساعات (يعني كل كشف ساعة)
            while (startTime.AddHours(1) <= endTime)
            {
                var nextSlot = startTime.AddHours(1);

                slots.Add(new DoctorAvilableTime
                {
                    DoctorId = request.DoctorId,
                    Day = request.Day,
                    StartTime = startTime,
                    EndTime = nextSlot,
                    IsBooked = false // الخانة اللي ضفناها في الموديل
                });

                startTime = nextSlot;
            }

            _context.DoctorAvilableTimes.AddRange(slots);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
