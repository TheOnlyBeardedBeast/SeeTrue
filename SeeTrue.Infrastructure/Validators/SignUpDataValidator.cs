using System;
using FluentValidation;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;

namespace SeeTrue.Infrastructure.Validators
{
    public class SignUpDataValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpDataValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.Language).NotEmpty().Must(x => Env.Languages.Contains(x.ToLower()));
        }
    }
}
