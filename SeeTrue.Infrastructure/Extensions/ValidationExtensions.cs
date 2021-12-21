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
    }
}
