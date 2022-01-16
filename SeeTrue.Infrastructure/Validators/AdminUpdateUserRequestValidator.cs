using System;
using FluentValidation;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Utils;

namespace SeeTrue.Infrastructure.Validators
{
    public class AdminUpdateUserRequestValidator : AbstractValidator<AdminUpdateUserRequest>
    {
        public AdminUpdateUserRequestValidator()
        {
            RuleFor(x => x.Email).EmailAddress().When(e => !string.IsNullOrWhiteSpace(e.Email));
            RuleFor(x => x.Password).MinimumLength(Env.MinimumPasswordLength).When(e => !string.IsNullOrWhiteSpace(e.Password));
            RuleFor(x => x.Language).Must(e => Env.Languages.Contains(e)).When(e => !string.IsNullOrWhiteSpace(e.Language));
            RuleFor(x => x.Role).Must(e => Env.AvailableRoles.Contains(e)).When(e => !string.IsNullOrWhiteSpace(e.Role));
            RuleFor(x => x.Audience).Must(e => Env.Audiences.Contains(e)).When(e => !string.IsNullOrWhiteSpace(e.Audience));
        }
    }
}
