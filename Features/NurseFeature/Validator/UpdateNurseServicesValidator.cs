using Features.NurseFeature.Command;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.NurseFeature.Validator
{
    public class UpdateNurseServicesValidator
     : AbstractValidator<AddNurseServicesCommand>
    {
        public UpdateNurseServicesValidator()
        {
            RuleFor(x => x.ServiceIds)
                .NotEmpty()
                .WithMessage("Please select at least one service.");

            RuleFor(x => x.ServiceIds)
                .Must(x => x.Distinct().Count() == x.Count)
                .WithMessage("Duplicate services are not allowed.");
        }
    }
}
