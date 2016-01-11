namespace Phundus.Specs.ContentTypes
{
    using Newtonsoft.Json;

    public class ChangeEmailAddressPostRequestContent
    {
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("newEmailAddress")]
        public string NewEmailAddress { get; set; }
    }

    public class ChangePasswordPostRequestContent
    {
        [JsonProperty("oldPassword")]
        public string OldPassword { get; set; }

        [JsonProperty("newPassword")]
        public string NewPassword { get; set; }
    }

    public class ResetPasswordPostRequestContent
    {
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
    }

    
}