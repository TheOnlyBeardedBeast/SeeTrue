using FluentValidation;
using SeeTrue.Infrastructure.Types;

namespace SeeTrue.Infrastructure.Validators
{
    public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator()
        {
            RuleFor(x => x).Must(x => string.IsNullOrWhiteSpace(x.Email) || string.IsNullOrWhiteSpace(x.Password) || x.UserMetaData != null);
        }
    }
}