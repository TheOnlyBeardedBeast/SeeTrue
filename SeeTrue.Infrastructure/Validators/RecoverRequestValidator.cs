using FluentValidation;
using SeeTrue.Infrastructure.Types;

namespace SeeTrue.Infrastructure.Validators
{
    public class RecoverRequestValidator : AbstractValidator<RecoverRequest>
    {
        public RecoverRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}