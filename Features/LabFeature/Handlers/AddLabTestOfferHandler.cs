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
    public class AddLabTestOfferHandler : IRequestHandler<AddLabTestOfferCommand, ResultResponse<string>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor http;

        public AddLabTestOfferHandler(ApplicationDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            this.http = http;
        }

        public async Task<ResultResponse<string>> Handle(AddLabTestOfferCommand request, CancellationToken cancellationToken)
        {
            // ID بتاع الحساب هو نفسه ID بتاع المعمل (زي ما عملنا في التسجيل)
            var labId = http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var lab = await _context.Labs.FirstOrDefaultAsync(l => l.ID == labId, cancellationToken);
            if (lab == null)
                return new ResultResponse<string> { ISucsses = false, Message = "المعمل غير موجود" };

            if (lab.Status != ConfrmationStatus.Approved)
                return new ResultResponse<string> { ISucsses = false, Message = "المعمل لسه مش متفعل من الأدمن" };

            var test = await _context.MedicalTests.FindAsync(new object[] { request.MedicalTestId }, cancellationToken);
            if (test == null)
                return new ResultResponse<string> { ISucsses = false, Message = "التحليل غير موجود في الكتالوج" };

            var alreadyAdded = await _context.LabTestOffers
                .AnyAsync(o => o.LabId == labId && o.MedicalTestId == request.MedicalTestId, cancellationToken);
            if (alreadyAdded)
                return new ResultResponse<string> { ISucsses = false, Message = "التحليل ده مضاف بالفعل عندك" };

            var offer = new LabTestOffer
            {
                LabId = labId!,
                MedicalTestId = request.MedicalTestId,
                Price = request.Price,
                IsAvailable = true
            };

            _context.LabTestOffers.Add(offer);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "تم إضافة التحليل لمعملك بنجاح",
                Obj = offer.ID
            };
        }
    }
}