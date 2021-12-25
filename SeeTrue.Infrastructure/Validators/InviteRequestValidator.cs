using System;
using FluentValidation;
using SeeTrue.Infrastructure.Types;

namespace SeeTrue.Infrastructure.Validators
{
    public class InviteRequestValidator : AbstractValidator<InviteRequest>
    {
        public InviteRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
