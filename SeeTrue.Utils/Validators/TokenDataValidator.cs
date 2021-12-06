using System;
using FluentValidation;
using SeeTrue.Utils.Types;

namespace SeeTrue.Utils.Validators
{
    public class TokenDataValidator : AbstractValidator<TokenData>
    {
        public TokenDataValidator()
        {
            RuleFor(x => x.GrantType).NotEmpty().Must(x => x == "password" || x == "refresh_token");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().When(x => x.GrantType == "password");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).When(x => x.GrantType == "password");
            RuleFor(x => x.RefreshToken).NotEmpty().When(x => x.GrantType == "refresh_token");
        }
    }
}
