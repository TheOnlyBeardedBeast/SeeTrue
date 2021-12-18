using System;
using SeeTrue.Infrastructure.Types;
using SeeTrue.Infrastructure.Validators;

namespace SeeTrue.Infrastructure.Extensions
{
    public static class ValidationExtensions
    {

        public static bool Validate(this SignUpData data)
        {
            var validator = new SignUpDataValidator();
            var result = validator.Validate(data);

            return result.IsValid;
        }

        public static bool Validate(this TokenData data)
        {
            var validator = new TokenDataValidator();
            var result = validator.Validate(data);

            return result.IsValid;
        }
    }
}
