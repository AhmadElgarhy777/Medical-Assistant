using FluentValidation;
using Features.RadiologyFeature.Commands;

namespace Features.RadiologyFeature.Validators
{
  

    public class UpdateRadiologyProfileCommandValidator : AbstractValidator<UpdateRadiologyProfileCommand>
    {
        public UpdateRadiologyProfileCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.AreaId).NotEmpty();
        }
    }

    public class AddRadiologyScanCommandValidator : AbstractValidator<AddRadiologyScanCommand>
    {
        public AddRadiologyScanCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        }
    }

    public class UpdateRadiologyScanCommandValidator : AbstractValidator<UpdateRadiologyScanCommand>
    {
        public UpdateRadiologyScanCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        }
    }

    public class BookRadiologyScanCommandValidator : AbstractValidator<BookRadiologyScanCommand>
    {
        public BookRadiologyScanCommandValidator()
        {
            RuleFor(x => x.RadiologyCenterId).NotEmpty();
            RuleFor(x => x.ScanId).NotEmpty().WithMessage("A scan must be selected");
            RuleFor(x => x.ScheduledDate).GreaterThanOrEqualTo(DateTime.Today).WithMessage("Scheduled date must be in the future");
            RuleFor(x => x.ScheduledTimeSlot).NotEmpty();
        }
    }

    public class UploadRadiologyReportCommandValidator : AbstractValidator<UploadRadiologyReportCommand>
    {
        public UploadRadiologyReportCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.ReportFile).NotNull().WithMessage("Report PDF is required");
        }
    }
}
