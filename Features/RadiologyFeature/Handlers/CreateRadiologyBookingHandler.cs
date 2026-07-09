using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess;
using Features.RadiologyFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using System.Security.Claims;

namespace Features.RadiologyFeature.Handlers
{
    public class CreateRadiologyBookingHandler : IRequestHandler<CreateRadiologyBookingCommand, ResultResponse<string>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor http;

        public CreateRadiologyBookingHandler(ApplicationDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            this.http = http;
        }

        public async Task<ResultResponse<string>> Handle(CreateRadiologyBookingCommand request, CancellationToken cancellationToken)
        {
            var patientId = http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (patientId == null)
                return new ResultResponse<string> { ISucsses = false, Message = "المريض غير مسجل دخول" };

            if (request.RadiologyScanIds == null || !request.RadiologyScanIds.Any())
                return new ResultResponse<string> { ISucsses = false, Message = "لازم تختار فحص واحد على الأقل" };

            var booking = new LabBooking
            {
                PatientId = patientId,
                ServiceType = ServiceTypeEnum.Radiology,
                VisitType = VisitTypeEnum.AtCenter, // الأشعة مش متاحة منزليًا
                RadiologyCenterId = request.RadiologyCenterId,
                ScheduledDate = request.ScheduledDate,
                ScheduledTimeSlot = request.ScheduledTimeSlot,
                PaymentMethod = request.PaymentMethod,
                Status = LabBookingStatusEnum.Confirmed
            };

            decimal subTotal = 0;

            foreach (var scanId in request.RadiologyScanIds)
            {
                var offer = await _context.RadiologyCenterScans
                    .Include(o => o.RadiologyScan)
                    .FirstOrDefaultAsync(o => o.RadiologyCenterId == request.RadiologyCenterId && o.RadiologyScanId == scanId, cancellationToken);

                if (offer == null)
                    return new ResultResponse<string> { ISucsses = false, Message = "الفحص غير متاح في هذا المركز" };

                var price = offer.Price ?? offer.RadiologyScan.BasePrice;
                booking.Items.Add(new LabBookingItem { RadiologyScanId = scanId, Price = price });
                subTotal += price;
            }

            booking.SubTotal = subTotal;
            booking.TotalPrice = subTotal - booking.DiscountAmount;

            _context.LabBookings.Add(booking);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "تم تأكيد حجز الأشعة بنجاح",
                Obj = booking.ID
            };
        }
    }
}