using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.RejectUploadedScan
{
    public class RejectUploadedScanValidator
     : AbstractValidator<RejectUploadedScanCommand>
    {
        public RejectUploadedScanValidator()
        {
            RuleFor(x => x.ScanRequestId)
                .NotEmpty();

            RuleFor(x => x.RejectReason)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
