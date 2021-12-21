using System;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Validators;

namespace SeeTrue.Infrastructure.Extensions
{
    public static class ValidationExtensions
    {

        public static bool Validate(this SignUpRequest data)
        {
            var validator = new SignUpDataValidator();
            var result = validator.Validate(data);

            return result.IsValid;
        }

        public static bool Validate(this TokenRequest data)
        {
            var validator = new TokenRequestValidator();
            var result = validator.Validate(data);

            return result.IsValid;
        }

        public static bool Validate(this VerifyRequest data)
        {
            var validator = new VerifyRequestValidator();
            var result = validator.Validate(data);

            return result.IsValid;
        }

        public static bool Validate(this MagicLinkRequest data)
        {
            var validator = new MagicLinkRequestValidator();
            var result = validator.Validate(data);

            return result.IsValid;
        }

        public static bool Validate(this RecoverRequest data)
        {
            var validator = new RecoverRequestValidator();
            var result = validator.Validate(data);

            return result.IsValid;
        }

        public static bool Validate(this UserUpdateRequest data)
        {
            var validator = new UserUpdateRequestValidator();
            var result = validator.Validate(data);

            return result.IsValid;
        }
    }
}
