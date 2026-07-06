using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess;
using Features.AdminFeature.Commands;
using MediatR;
using Models;

namespace Features.AdminFeature.Handlers
{
    public class CreateMedicalTestHandler : IRequestHandler<CreateMedicalTestCommand, ResultResponse<string>>
    {
        private readonly ApplicationDbContext _context;
        public CreateMedicalTestHandler(ApplicationDbContext context) => _context = context;

        public async Task<ResultResponse<string>> Handle(CreateMedicalTestCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return new ResultResponse<string> { ISucsses = false, Message = "اسم التحليل مطلوب" };

            var exists = _context.MedicalTests.Any(t => t.Name == request.Name);
            if (exists)
                return new ResultResponse<string> { ISucsses = false, Message = "التحليل ده موجود بالفعل في الكتالوج" };

            var test = new MedicalTest
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                BasePrice = request.BasePrice,
                TurnaroundHours = request.TurnaroundHours,
                PreparationInstructions = request.PreparationInstructions,
                RequiresFasting = request.RequiresFasting
            };

            _context.MedicalTests.Add(test);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = "تم إضافة التحليل للكتالوج بنجاح",
                Obj = test.ID
            };
        }
    }
}