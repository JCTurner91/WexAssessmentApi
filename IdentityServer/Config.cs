// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

// Majority of IdentityServer code pulled from the QuickStart guide in the official Duende IdentityServer documentation.
// QuickGuide found here: https://docs.duendesoftware.com/identityserver/v6/quickstarts/1_client_credentials/

using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("api1", "My API")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "client",

                // no interactive user, use the clientid/secret for authentication
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                // secret for authentication
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                // scopes that client has access to
                AllowedScopes = { "api1" }
            }
        };
}