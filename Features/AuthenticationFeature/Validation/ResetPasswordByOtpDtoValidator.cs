using Features.AuthenticationFeature.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features.AuthenticationFeature.Validation
{
    public class ResetPasswordByOtpDtoValidator : AbstractValidator<ResetPasswordByOtpCommand>
    {
        public ResetPasswordByOtpDtoValidator()
        {
            RuleFor(x => x.ResetDto.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(x => x.ResetDto.Otp)
                .NotEmpty()
                .WithMessage("OTP is required.")
                .Length(6)
                .WithMessage("OTP must be exactly 6 digits.")
                .Matches(@"^\d{6}$")
                .WithMessage("OTP must contain only digits.");

            RuleFor(x => x.ResetDto.NewPassword)
                .NotEmpty()
                .WithMessage("New password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters.")
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d")
                .WithMessage("Password must contain at least one number.")
                .Matches(@"[!@#$%^&*(),.?""':{}|<>]")
                .WithMessage("Password must contain at least one special character.");
        }
    }
}
