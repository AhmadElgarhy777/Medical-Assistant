using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.RegisterationFeature.Commands;
using MediatR;
using Models.Enums;

namespace Features.RegisterationFeature.Handelers
{
    public class ApproveLabHandler : IRequestHandler<ApproveLabCommand, ResultResponse<string>>
    {
        private readonly ILabRepository _labRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ApproveLabHandler(ILabRepository labRepository, IUnitOfWork unitOfWork)
        {
            _labRepository = labRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultResponse<string>> Handle(ApproveLabCommand request, CancellationToken cancellationToken)
        {
            var lab = await _labRepository.GetLabByIdAsync(request.LabId);
            if (lab == null)
                return new ResultResponse<string> { ISucsses = false, Message = "المعمل غير موجود" };

            // lab.Status = request.Approve ? ConfrmationStatus.Accepted : ConfrmationStatus.Rejected;
            lab.Status = request.Approve ? ConfrmationStatus.Approved : ConfrmationStatus.Rejected;
            lab.IsActive = request.Approve;

            await _labRepository.UpdateLabAsync(lab);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return new ResultResponse<string>
            {
                ISucsses = true,
                Message = request.Approve ? "تم قبول المعمل وتفعيله" : "تم رفض المعمل"
            };
        }
    }
}