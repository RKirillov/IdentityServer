// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        //используем как identity ресурс, куда может быть доступ.
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>// 
            new List<ApiScope>
            { 
                new ApiScope("test-api", "My test Api")
            };

        public static IEnumerable<Client> Clients =>//те клиенты о которых identity server знает
            new List<Client> 
            { 
                new Client
                {
                    ClientId = "first-client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,//ClientCredentials- передаст ClientId, ClientSecrets
                    ClientSecrets = { new Secret("secret".Sha256())},
                    AllowedScopes = { "test-api" }//приложение получает доступы к этому скопу api используя свои ClientCredentials
                },
                new Client//с помощью этого сервера можем аутентифицироваться и получить доступ к OpenId и Profile пользователя.
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,//возвращаем авторизационный код, говорит куда редиректить

                    // where to redirect to after login
                    RedirectUris = { "https://localhost:7120/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:7120/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
    }
}