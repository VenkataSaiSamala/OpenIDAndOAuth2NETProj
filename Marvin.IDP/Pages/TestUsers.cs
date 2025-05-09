// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using IdentityModel;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;

namespace Marvin.IDP;

public static class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = "69118",
                country = "Germany"
            };
                
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "7307ac72-d9cc-4981-b778-0be6dacc7c8c",
                    Username = "alissa",
                    Password = "password",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Alissa South"),
                        new Claim(JwtClaimTypes.GivenName, "Alissa"),
                        new Claim(JwtClaimTypes.FamilyName, "South")
                    }
                },
                new TestUser
                {
                    SubjectId = "a92e79b7-e0c4-49c0-ab27-51d4491f0617",
                    Username = "bobby",
                    Password = "password",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Bobby South"),
                        new Claim(JwtClaimTypes.GivenName, "Bobby"),
                        new Claim(JwtClaimTypes.FamilyName, "South")
                    }
                }
            };
        }
    }
}