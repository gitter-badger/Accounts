using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Thinktecture.IdentityModel;

namespace Accounts
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "accountsclient",
                Authority = EndPoints.AuthAddress,
                RedirectUri = EndPoints.CallbackEndpoint,
                PostLogoutRedirectUri = EndPoints.BaseAddress,
                //ResponseType = "code id_token token",
                //Scope = "roles",
                //Scope = "openid email profile read write offline_access",

                SignInAsAuthenticationType = "Cookies",

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthorizationCodeReceived = async n =>
                    {
                        // filter "protocol" claims
                        var claims = new List<System.Security.Claims.Claim>(from c in n.AuthenticationTicket.Identity.Claims
                                                                            where c.Type != "iss" &&
                                                                                  c.Type != "aud" &&
                                                                                  c.Type != "nbf" &&
                                                                                  c.Type != "exp" &&
                                                                                  c.Type != "iat" &&
                                                                                  c.Type != "nonce" &&
                                                                                  c.Type != "c_hash" &&
                                                                                  c.Type != "at_hash"
                                                                            select c);

                        //// get userinfo data
                        //var userInfoClient = new UserInfoClient(
                        //    new Uri(Constants.UserInfoEndpoint),
                        //    n.ProtocolMessage.AccessToken);

                        //var userInfo = await userInfoClient.GetAsync();
                        //userInfo.Claims.ToList().ForEach(ui => claims.Add(new Claim(ui.Item1, ui.Item2)));

                        //// get access and refresh token
                        //var tokenClient = new OAuth2Client(
                        //    new Uri(Constants.TokenEndpoint),
                        //    "katanaclient",
                        //    "secret");

                        //var response = await tokenClient.RequestAuthorizationCodeAsync(n.Code, n.RedirectUri);

                        //claims.Add(new Claim("access_token", response.AccessToken));
                        //claims.Add(new Claim("expires_at", DateTime.Now.AddSeconds(response.ExpiresIn).ToLocalTime().ToString()));
                        //claims.Add(new Claim("refresh_token", response.RefreshToken));
                        claims.Add(new System.Security.Claims.Claim("id_token", n.ProtocolMessage.IdToken));

                        n.AuthenticationTicket = new AuthenticationTicket(new ClaimsIdentity(claims.Distinct(new ClaimComparer()), n.AuthenticationTicket.Identity.AuthenticationType), n.AuthenticationTicket.Properties);
                    },

                    RedirectToIdentityProvider = async n =>
                    {
                        // if signing out, add the id_token_hint
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token").Value;
                            n.ProtocolMessage.IdTokenHint = idTokenHint;
                        }
                    },
                }
            });

        }
    }

    public static class EndPoints
    {
        public static string BaseAddress  =         WebConfigurationManager.AppSettings["accounts:baseUrl"];
        public static string AuthAddress  =         WebConfigurationManager.AppSettings["accounts:authUrl"].TrimEnd('/');
        public static string IdentityApiAddress =   WebConfigurationManager.AppSettings["accounts:identityApiUrl"];
        public static string PeopleCloudAddress = WebConfigurationManager.AppSettings["accounts:peopleCloudUrl"];
        public static string CdnAddress = WebConfigurationManager.AppSettings["accounts:cdnUrl"];

        public static string AuthorizeEndpoint = AuthAddress + "/connect/authorize";
        public static string LogoutEndpoint = AuthAddress + "/connect/endsession";
        public static string TokenEndpoint = AuthAddress + "/connect/token";
        public static string UserInfoEndpoint = AuthAddress + "/connect/userinfo";
        public static string IdentityTokenValidationEndpoint = AuthAddress + "/connect/identitytokenvalidation";

        public static string CallbackEndpoint = BaseAddress;

        public const string AspNetWebApiSampleApi = "http://localhost:2727/";
    }
}