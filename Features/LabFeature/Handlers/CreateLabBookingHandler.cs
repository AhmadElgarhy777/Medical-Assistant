using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess;
using Features.LabFeature.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Enums;
using System.Security.Claims;

namespace Features.LabFeature.Handlers
{
    public class CreateLabBookingHandler : IRequestHandler<CreateLabBookingCommand, ResultResponse<string>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor http;

        public CreateLabBookingHandler(ApplicationDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            this.http = http;
        }

        public async Task<ResultResponse<string>> Handle(CreateLabBookingCommand request, CancellationToken cancellationToken)
        {
            var patientId = http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (patientId == null)
                return new ResultResponse<string> { ISucsses = false, Message = "المريض غير مسجل دخول" };

            if (request.VisitType == VisitTypeEnum.AtCenter && string.IsNullOrEmpty(request.LabId))
                return new ResultResponse<string> { ISucsses = false, Message = "لازم تحدد المعمل" };

            if (request.VisitType == VisitTypeEnum.HomeCollection && string.IsNullOrEmpty(request.HomeAddress))
                return new ResultResponse<string> { ISucsses = false, Message = "العنوان مطلوب للمعاينة المنزلية" };

            if (request.MedicalTestIds == null || !request.MedicalTestIds.Any())
                return new ResultResponse<string> { ISucsses = false, Message = "لازم تختار تحليل واحد على الأقل" };

            var booking = new LabBooking
            {
                PatientId = patientId,
                ServiceType = ServiceTypeEnum.Lab,
                VisitType = request.VisitType,
                LabId = request.LabId,
                AreaId = request.AreaId,
                HomeAddress = request.HomeAddress,
                ScheduledDate = request.ScheduledDate,
                ScheduledTimeSlot = request.ScheduledTimeSlot,
                PaymentMethod = request.PaymentMethod,
                Status = LabBookingStatusEnum.Confirmed
            };

            decimal subTotal = 0;

            foreach (var testId in request.MedicalTestIds)
            {
                decimal price;

                if (!string.IsNullOrEmpty(request.LabId))
                {
                    var offer = await _context.LabTestOffers
                        .Include(o => o.MedicalTest)
                        .FirstOrDefaultAsync(o => o.LabId == request.LabId && o.MedicalTestId == testId, cancellationToken);

                    if (offer == null)
                        return new ResultResponse<string> { ISucsses = false, Message = $"التحليل غير متاح في هذا المعمل" };

                    price = offer.Price ?? offer.MedicalTest.BasePrice;
                }
                else
                {
                    var test = await _context.MedicalTests.FindAsync(new object[] { testId }, cancellationToken);
                    if (test == null)
                        return new ResultResponse<string> { ISucsses = false, Message = "التحليل غير موجود" };

                    price = test.BasePrice;
                }

                booking.Items.Add(new LabBookingItem { MedicalTestId = testId, Price = price });
                subTotal += price;
            }

            booking.HomeCollectionFee = request.VisitType == VisitTypeEnum.HomeCollection ? 50 : 0;
            booking.SubTotal = subTotal;
            booking.TotalPrice = subTotal + booking.HomeCollectionFee - booking.DiscountAmount;

            _context.LabBookings.Add(booking);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "تم تأكيد الحجز بنجاح",
                Obj = booking.ID
            };
        }
    }
}