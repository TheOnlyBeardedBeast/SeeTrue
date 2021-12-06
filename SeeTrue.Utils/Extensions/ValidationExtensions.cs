using System;
using SeeTrue.Utils.Types;
using SeeTrue.Utils.Validators;

namespace SeeTrue.Utils.Extensions
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
