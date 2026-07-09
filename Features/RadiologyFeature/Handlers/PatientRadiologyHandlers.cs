using AutoMapper;
using DataAccess;
using Features.RadiologyFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Security.Claims;
using Models.Enums;

namespace Features.RadiologyFeature.Handlers
{
    public class PatientRadiologyHandlers :
        IRequestHandler<BookRadiologyScanCommand, ResultResponse<string>>,
        IRequestHandler<CancelPatientRadiologyBookingCommand, ResultResponse<string>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientRadiologyHandlers(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string? GetPatientId() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<ResultResponse<string>> Handle(BookRadiologyScanCommand request, CancellationToken cancellationToken)
        {
            var patientId = GetPatientId();
            if (string.IsNullOrEmpty(patientId)) return new ResultResponse<string> { ISucsses = false, Message = "Unauthorized" };

            var center = await _context.RadiologyCenters.FirstOrDefaultAsync(l => l.ID == request.RadiologyCenterId && !l.IsDeleted, cancellationToken);
            if (center == null) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center not found" };
            if (center.Status != ConfrmationStatus.Approved) return new ResultResponse<string> { ISucsses = false, Message = "Radiology Center is not currently approved" };

            // Phase 3: Booking Validation - Check Schedule
            var requestedDay = request.ScheduledDate.DayOfWeek;
            
            if (!TimeSpan.TryParse(request.ScheduledTimeSlot, out var requestedTime))
            {
                return new ResultResponse<string> { ISucsses = false, Message = "Invalid time format. Please use HH:mm" };
            }

            var schedule = await _context.RadiologySchedules.FirstOrDefaultAsync(
                s => s.RadiologyCenterId == request.RadiologyCenterId && 
                     !s.IsDeleted && 
                     s.IsAvailable && 
                     s.DayOfWeek == requestedDay &&
                     s.FromTime <= requestedTime &&
                     s.ToTime >= requestedTime, 
                cancellationToken);

            if (schedule == null)
            {
                return new ResultResponse<string> { ISucsses = false, Message = "The selected time is not within the Center's available schedule for this day." };
            }

            // Phase 3: Check Conflicts (Optional but requested: "أن الموعد غير محجوز إذا كان النظام يمنع التكرار")
            var existingBooking = await _context.LabBookings.FirstOrDefaultAsync(
                b => b.RadiologyCenterId == request.RadiologyCenterId && 
                     b.ScheduledDate.Date == request.ScheduledDate.Date && 
                     b.ScheduledTimeSlot == request.ScheduledTimeSlot &&
                     !b.IsDeleted &&
                     b.Status != LabBookingStatusEnum.Cancelled && 
                     b.Status != LabBookingStatusEnum.Rejected,
                cancellationToken);

            if (existingBooking != null)
            {
                return new ResultResponse<string> { ISucsses = false, Message = "This time slot is already booked." };
            }

            var scanOffer = await _context.RadiologyCenterScans.FirstOrDefaultAsync(t => t.RadiologyScanId == request.ScanId && t.RadiologyCenterId == request.RadiologyCenterId && !t.IsDeleted, cancellationToken);
            if (scanOffer == null)
            {
                return new ResultResponse<string> { ISucsses = false, Message = "Selected scan is not available at this center" };
            }

            var booking = new LabBooking
            {
                PatientId = patientId,
                ServiceType = ServiceTypeEnum.Radiology,
                VisitType = VisitTypeEnum.AtCenter, // Center Visit
                RadiologyCenterId = request.RadiologyCenterId,
                ScheduledDate = request.ScheduledDate,
                ScheduledTimeSlot = request.ScheduledTimeSlot,
                Status = LabBookingStatusEnum.Pending,
                IsPaid = false,
                PaymentMethod = "Cash",
                Items = new List<LabBookingItem>
                {
                    new LabBookingItem
                    {
                        RadiologyScanId = request.ScanId,
                        Price = scanOffer.Price ?? 0
                    }
                },
                SubTotal = scanOffer.Price ?? 0,
                TotalPrice = scanOffer.Price ?? 0
            };

            _context.LabBookings.Add(booking);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Message = "Appointment created successfully", Obj = booking.ID };
        }

        public async Task<ResultResponse<string>> Handle(CancelPatientRadiologyBookingCommand request, CancellationToken cancellationToken)
        {
            var patientId = GetPatientId();
            if (string.IsNullOrEmpty(patientId)) return new ResultResponse<string> { ISucsses = false, Message = "Unauthorized" };

            var booking = await _context.LabBookings.FirstOrDefaultAsync(b => b.ID == request.Id && b.PatientId == patientId && !b.IsDeleted, cancellationToken);
            if (booking == null) return new ResultResponse<string> { ISucsses = false, Message = "Appointment not found" };

            if (booking.Status == LabBookingStatusEnum.Completed || booking.Status == LabBookingStatusEnum.Processing)
            {
                return new ResultResponse<string> { ISucsses = false, Message = "Cannot cancel an appointment that is already processing or completed." };
            }

            booking.Status = LabBookingStatusEnum.Cancelled;
            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Message = "Appointment cancelled successfully" };
        }
    }
}
