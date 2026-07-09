using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.PatientScanRequet.Commands.CreateScanRequest
{
    public class CreateScanRequestCommandValidator
    : AbstractValidator<CreateScanRequestCommand>
    {
        public CreateScanRequestCommandValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty();

            RuleFor(x => x.AIModelType)
                .IsInEnum();

            RuleFor(x => x.ExpirationDate)
                .Must(x => x == null || x > DateTime.UtcNow)
                .WithMessage("Expiration date must be in the future.");
        }
    }
}
