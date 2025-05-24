using System.Net.Http;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace ImageGallery.Client.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthenticationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        // GET: AuthenticationController
        [Authorize]
        public async Task Logout()
        {
            var client = _httpClientFactory.CreateClient("IDPClient");
            var discoverDocumentRespose = await client.GetDiscoveryDocumentAsync();

            if (discoverDocumentRespose.IsError)
                throw new Exception(discoverDocumentRespose.Error);

            var accessTokenRevocationResponse = await client
                .RevokeTokenAsync(new()
                {
                    Address = discoverDocumentRespose.RevocationEndpoint,
                    ClientId = "imagegalleryclient",
                    ClientSecret = "secret",
                    Token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken)
                });
            if (accessTokenRevocationResponse.IsError)
                throw new Exception(accessTokenRevocationResponse.Error);

            var refreshTokenRevocationResponse = await client
                .RevokeTokenAsync(new()
                {
                    Address = discoverDocumentRespose.RevocationEndpoint,
                    ClientId = "imagegalleryclient",
                    ClientSecret = "secret",
                    Token = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken)
                });
            if (refreshTokenRevocationResponse.IsError)
                throw new Exception(refreshTokenRevocationResponse.Error);
            
            /*

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            */
        }

        public IActionResult AccessDenied()
        {

            return View();
        }

    }
}
