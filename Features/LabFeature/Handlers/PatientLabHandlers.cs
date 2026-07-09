using AutoMapper;
using DataAccess;
using Features.LabFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Security.Claims;
using Models.Enums;

namespace Features.LabFeature.Handlers
{
    public class PatientLabHandlers :
        IRequestHandler<BookLabTestCommand, ResultResponse<string>>,
        IRequestHandler<CancelPatientLabBookingCommand, ResultResponse<string>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientLabHandlers(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string? GetPatientId() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<ResultResponse<string>> Handle(BookLabTestCommand request, CancellationToken cancellationToken)
        {
            var patientId = GetPatientId();
            if (string.IsNullOrEmpty(patientId)) return new ResultResponse<string> { ISucsses = false, Message = "Unauthorized" };

            // Phase 3: Booking Validation
            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.ID == request.LabId && !l.IsDeleted, cancellationToken);
            if (lab == null) return new ResultResponse<string> { ISucsses = false, Message = "Lab not found" };
            if (lab.Status != ConfrmationStatus.Approved) return new ResultResponse<string> { ISucsses = false, Message = "Lab is not currently approved" };

            // Validate Schedule exists and is available
            var requestedDay = request.ScheduledDate.DayOfWeek;
            
            // Assuming ScheduledTimeSlot is passed as "HH:mm"
            if (!TimeSpan.TryParse(request.ScheduledTimeSlot, out var requestedTime))
            {
                return new ResultResponse<string> { ISucsses = false, Message = "Invalid time format. Please use HH:mm" };
            }

            var schedule = await _context.LabSchedules.FirstOrDefaultAsync(
                s => s.LabId == request.LabId && 
                     !s.IsDeleted && 
                     s.IsAvailable && 
                     s.DayOfWeek == requestedDay &&
                     s.FromTime <= requestedTime &&
                     s.ToTime >= requestedTime, 
                cancellationToken);

            if (schedule == null)
            {
                return new ResultResponse<string> { ISucsses = false, Message = "The selected time is not within the Lab's available schedule for this day." };
            }

            // Creating the Booking
            var booking = new LabBooking
            {
                PatientId = patientId,
                ServiceType = ServiceTypeEnum.Lab,
                VisitType = request.VisitType,
                LabId = request.LabId,
                ScheduledDate = request.ScheduledDate,
                ScheduledTimeSlot = request.ScheduledTimeSlot,
                HomeAddress = request.HomeAddress,
                Status = LabBookingStatusEnum.Pending,
                IsPaid = false,
                PaymentMethod = "Cash",
                Items = new List<LabBookingItem>()
            };

            decimal totalPrice = 0;

            foreach (var testId in request.TestIds)
            {
                var testOffer = await _context.LabTestOffers.FirstOrDefaultAsync(t => t.MedicalTestId == testId && t.LabId == request.LabId && !t.IsDeleted, cancellationToken);
                if (testOffer != null && testOffer.IsAvailable)
                {
                    booking.Items.Add(new LabBookingItem
                    {
                        MedicalTestId = testId,
                        Price = testOffer.Price ?? 0
                    });
                    totalPrice += testOffer.Price ?? 0;
                }
            }

            if (!booking.Items.Any())
                return new ResultResponse<string> { ISucsses = false, Message = "No valid tests selected" };

            if (request.VisitType == VisitTypeEnum.HomeCollection)
            {
                booking.HomeCollectionFee = 100; // arbitrary fee, can be configured later
                totalPrice += booking.HomeCollectionFee;
            }

            booking.SubTotal = totalPrice;
            booking.TotalPrice = totalPrice;

            _context.LabBookings.Add(booking);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Message = "Booking created successfully", Obj = booking.ID };
        }

        public async Task<ResultResponse<string>> Handle(CancelPatientLabBookingCommand request, CancellationToken cancellationToken)
        {
            var patientId = GetPatientId();
            if (string.IsNullOrEmpty(patientId)) return new ResultResponse<string> { ISucsses = false, Message = "Unauthorized" };

            var booking = await _context.LabBookings.FirstOrDefaultAsync(b => b.ID == request.Id && b.PatientId == patientId && !b.IsDeleted, cancellationToken);
            if (booking == null) return new ResultResponse<string> { ISucsses = false, Message = "Booking not found" };

            if (booking.Status == LabBookingStatusEnum.Completed || booking.Status == LabBookingStatusEnum.Processing)
            {
                return new ResultResponse<string> { ISucsses = false, Message = "Cannot cancel a booking that is already processing or completed." };
            }

            booking.Status = LabBookingStatusEnum.Cancelled;
            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string> { ISucsses = true, Message = "Booking cancelled successfully" };
        }
    }
}
