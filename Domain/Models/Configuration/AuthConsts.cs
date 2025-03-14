using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Configuration
{
    public static class AuthConsts
    {
        public const int UsernameMinLength = 5;

        public const string PasswordRegex = @"^(?=.*[A-Z])(?=.*[\W])(?=.*[0-9])(?=.*[a-z]).{6,128}$";
        public const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public const string UsernameLengthValidationError = "REGISTER_ERR_USERNAMEVALIDATION";
        public const string EmailValidationError = "REGISTER_ERR_EMAILVALIDATION";
        public const string PasswordValidationError = "REGISTER_ERR_PASSVALIDATION";
        public class LoginProviders
        {
            public const string Google = "GOOGLE";
            public const string Facebook = "FACEBOOK";
            public const string Password = "PASSWORD";
        }
    }
}
