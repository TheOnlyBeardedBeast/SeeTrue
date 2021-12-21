using FluentValidation;
using SeeTrue.Infrastructure.Types;

namespace SeeTrue.Infrastructure.Validators
{
    public class MagicLinkRequestValidator : AbstractValidator<MagicLinkRequest>
    {
        public MagicLinkRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}