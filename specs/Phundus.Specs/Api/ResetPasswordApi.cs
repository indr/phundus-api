namespace Phundus.Specs.Api
{
    public class ResetPasswordApi : ApiBase
    {
        public ResetPasswordApi() : base("account/reset-password")
        {
        }
    }

    public class ChangePasswordApi : ApiBase
    {
        public ChangePasswordApi() : base("account/change-password")
        {
        }
    }

    public class ChangeEmailAddressApi : ApiBase
    {
        public ChangeEmailAddressApi() : base("account/change-email-address")
        {
        }
    }

    public class ValidateApi : ApiBase
    {
        public ValidateApi() : base("account/validate")
        {
        }
    }
}