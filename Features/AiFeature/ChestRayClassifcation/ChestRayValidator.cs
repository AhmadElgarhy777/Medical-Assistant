using Features.AiFeature.AnalyzeBrainTumorFeature.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AiFeature.ChestRayClassifcation
{
    public class ChestRayValidatorCommandValidator
     : AbstractValidator<ChestRayClassifcationCommand>
    {
        public ChestRayValidatorCommandValidator()
        {
            RuleFor(x => x.Images)
                .NotEmpty();

            RuleForEach(x => x.Images)
                .Must(x => x.Length > 0)
                .WithMessage("Image is required.");

            RuleFor(x => x.PatientId)
                .NotEmpty();

            RuleFor(x => x.DoctorId)
                .NotEmpty();
        }
    }
}
