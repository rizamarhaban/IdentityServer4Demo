using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public class Config
    {
        private const string RIZA_API = "rizaapi";
        private const string CLIENT_SECRET = "67A20C10A94248DBA64B4F1EB00CFD6A";

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),

                new IdentityResource("custom", new[] { "status" })
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource(RIZA_API, "My Custom API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "Riza-WebApi",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = { new Secret(CLIENT_SECRET.Sha256()) },
                    AllowedScopes = { RIZA_API }
                },

                new Client
                {
                    ClientId = "Riza-WebApp",
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    RedirectUris = { "http://localhost:5000/signin-oidc" },
                    RequireConsent = false,

                    FrontChannelLogoutUri= "http://localhost:5000/signout-oidc",

                    ClientSecrets = { new Secret(CLIENT_SECRET.Sha256()) },
                    AllowedScopes = { "openid", "profile", RIZA_API }
                }
            };
        }
    }
}
