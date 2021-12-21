using FluentValidation;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;

namespace SeeTrue.Infrastructure.Validators
{
    public class TokenRequestValidator : AbstractValidator<TokenRequest>
    {
        public TokenRequestValidator()
        {
            RuleFor(x => x.GrantType).NotEmpty().Must(x => x == "password" || x == "refresh_token");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().When(x => x.GrantType == "password");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(Env.MinimumPasswordLength).When(x => x.GrantType == "password");
            RuleFor(x => x.RefreshToken).NotEmpty().When(x => x.GrantType == "refresh_token");
        }
    }
}
