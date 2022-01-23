using FluentValidation;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;

namespace SeeTrue.Infrastructure.Validators
{
    public class VerifyRequestValidator : AbstractValidator<VerifyRequest>
    {
        public VerifyRequestValidator()
        {
            RuleFor(x => x.Type).Must(t => t == "signup" || t == "recovery" || t == "invite");
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(Env.MinimumPasswordLength).When(p => !string.IsNullOrWhiteSpace(p.Password));
            RuleFor(x => x.Name).NotEmpty().When(p => p.Type == "invite");
        }
    }
}