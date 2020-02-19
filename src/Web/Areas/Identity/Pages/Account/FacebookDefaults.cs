namespace Microsoft.eShopWeb.Web.Areas.Identity.Pages.Account
{
    public static class FacebookDefaults
    {
        public const string AuthenticationScheme = "Facebook";

        public static readonly string DisplayName = "Facebook";

        // https://developers.facebook.com/docs/facebook-login/manually-build-a-login-flow#login
        public static readonly string AuthorizationEndpoint = "https://www.facebook.com/v4.0/dialog/oauth";

        public static readonly string TokenEndpoint = "https://graph.facebook.com/v4.0/oauth/access_token";

        public static readonly string UserInformationEndpoint = "https://graph.facebook.com/v4.0/me";
    }
}