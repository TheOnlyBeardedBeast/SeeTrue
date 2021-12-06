using System;
using FluentValidation;
using SeeTrue.Utils.Types;

namespace SeeTrue.Utils.Validators
{
    public class SignUpDataValidator : AbstractValidator<SignUpData>
    {
        public SignUpDataValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }
}
