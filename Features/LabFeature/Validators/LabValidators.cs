using FluentValidation;
using Features.LabFeature.Commands;

namespace Features.LabFeature.Validators
{


    public class UpdateLabProfileCommandValidator : AbstractValidator<UpdateLabProfileCommand>
    {
        public UpdateLabProfileCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.AreaId).NotEmpty();
        }
    }

    public class AddLabTestCommandValidator : AbstractValidator<AddLabTestCommand>
    {
        public AddLabTestCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.EstimatedTime).NotEmpty();
        }
    }

    public class UpdateLabTestCommandValidator : AbstractValidator<UpdateLabTestCommand>
    {
        public UpdateLabTestCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.EstimatedTime).NotEmpty();
        }
    }

    public class AddLabScheduleCommandValidator : AbstractValidator<AddLabScheduleCommand>
    {
        public AddLabScheduleCommandValidator()
        {
            RuleFor(x => x.DayOfWeek).IsInEnum();
            RuleFor(x => x.FromTime).LessThan(x => x.ToTime).WithMessage("FromTime must be before ToTime");
        }
    }

    public class UpdateLabScheduleCommandValidator : AbstractValidator<UpdateLabScheduleCommand>
    {
        public UpdateLabScheduleCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.DayOfWeek).IsInEnum();
            RuleFor(x => x.FromTime).LessThan(x => x.ToTime).WithMessage("FromTime must be before ToTime");
        }
    }

    public class BookLabTestCommandValidator : AbstractValidator<BookLabTestCommand>
    {
        public BookLabTestCommandValidator()
        {
            RuleFor(x => x.LabId).NotEmpty();
            RuleFor(x => x.VisitType).IsInEnum();
            RuleFor(x => x.TestIds).NotEmpty().WithMessage("At least one test must be selected");
            RuleFor(x => x.ScheduledDate).GreaterThanOrEqualTo(DateTime.Today).WithMessage("Scheduled date must be in the future");
            RuleFor(x => x.ScheduledTimeSlot).NotEmpty();
            RuleFor(x => x.HomeAddress).NotEmpty().When(x => x.VisitType == Models.Enums.VisitTypeEnum.HomeCollection)
                .WithMessage("Home address is required for Home Collection");
        }
    }

    public class AssignCollectorCommandValidator : AbstractValidator<AssignCollectorCommand>
    {
        public AssignCollectorCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CollectorId).NotEmpty().WithMessage("Collector ID is required");
        }
    }
}
