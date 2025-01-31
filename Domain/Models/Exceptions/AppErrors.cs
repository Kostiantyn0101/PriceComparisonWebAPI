using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions
{
    public static class AppErrors
    {
        /// <summary>
        /// Error codes related to auth
        /// </summary>
        public static class Auth
        {
            public static string EmailExists => "REGISTER_ERR_EMAILEXISTS";
            public static string UserNameExists => "REGISTER_ERR_USERNAMEEXISTS";
            public static string UserNotFound => "LOGIN_ERR_USERNOTFOUND";
            public static string PasswordIncorrect => "LOGIN_ERR_PASSINCORRECT";
            public static string ProviderMismatch => "LOGIN_ERR_PROVIDERMISMATCH";
            public static string ProviderNotSupported => "LOGIN_ERR_PROVIDERNOTSUPPORTED";
            public static string InvalidAccessToken => "LOGIN_ERR_INVALIDTOKEN";
            public static string SocialRegistrationFailed => "REGISTER_ERR_SOCIALFAILED";
            public static string GoogleConfigInvalid => "LOGIN_ERR_GOOGLECONFIGINVALID";
            public static string AddToRoleFailed => "REGISTER_ERR_ADDTOROLEFAILED";
            public static string PathNotFound => "REGISTER_ERR_PATHNOTFOUND";
            public static string RefreshInvalidToken => "REFRESH_ERR_INVALIDTOKEN";
            public static string RefreshTokenMismatch => "REFRESH_ERR_REFRESHTOKENINVALID";
            public static string RefreshTokenExpired => "REFRESH_ERR_REFRESHTOKENEXPIRED";
            public static string ChangePasswordFailed => "PASS_ERR_CHANGEPASSFAILED";
            public static string ResetPasswordFailed => "PASS_ERR_RESETPASSFAILED";
        }

        /// <summary>
        /// Error codes related to general errors like unexpected exceptions
        /// </summary>
        public static class General
        {
            public static string InternalServerError => "UNEXPECTED_ERR_INTERNALSERVERERROR";
            public static string CreateError => "CREATE_ERROR";
            public static string UpdateError => "UPDATE_ERROR";
            public static string DeleteError => "DELETE_ERROR";
            public static string NotFound => "NOTFOUND_ERROR";
            public static string UploadError => "UPLOAD_ERROR";
            public static string MediaTypeError => "MEDIA_TYPE_ERROR";
            public static string SizeFileError => "SIZE_FILE_ERROR";
        }

        public static class BybitApiKey
        {
            public const string ApiKeyError = "BYBIT_APIKEY_ERR_INVALID";

            public const string ApiKeyExpired = "BYBIT_APIKEY_ERR_EXPIRED";

            public const string ApiKeySecretError = "BYBIT_APIKEY_ERR_SECRET_INVALID";

            public const string ApiKeyDuplicate = "BYBIT_APIKEY_ERR_DUPLICATE";
        }
    }
}
