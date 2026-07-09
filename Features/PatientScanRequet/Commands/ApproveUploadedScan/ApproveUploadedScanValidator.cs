using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.ApproveUploadedScan
{
    public class ApproveUploadedScanValidator
     : AbstractValidator<ApproveUploadedScanCommand>
    {
        public ApproveUploadedScanValidator()
        {
            RuleFor(x => x.ScanRequestId)
                .NotEmpty();
        }
    }
}
