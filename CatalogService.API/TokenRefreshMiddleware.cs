using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CatalogService.API
{
    public class TokenRefreshMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tokenEndpoint;

        public TokenRefreshMiddleware(RequestDelegate next, string clientId, string clientSecret, string tokenEndpoint)
        {
            _next = next;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _tokenEndpoint = tokenEndpoint;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                var existingToken = authorizationHeader.ToString().Replace("Bearer ", string.Empty);

                var client = new HttpClient();
                var tokenResponse = await client.RequestTokenAsync(new TokenRequest
                {
                    Address = _tokenEndpoint,
                    ClientId = _clientId,
                    ClientSecret = _clientSecret,
                    GrantType = "client_credentials",

                });

                if (!tokenResponse.IsError)
                {
                    var newToken = $"Bearer {tokenResponse.AccessToken}";
                    context.Request.Headers["Authorization"] = $"Bearer {tokenResponse.AccessToken}";
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(tokenResponse.AccessToken) as JwtSecurityToken;

                    var claims = jsonToken.Claims; // Update with your actual claims
                    var identity = new ClaimsIdentity(claims, "Bearer");
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, "Bearer");

                    context.User = principal;
                    context.Request.HttpContext.User = principal;

                    // Log the scopes
                    var scopes = jsonToken?.Claims.FirstOrDefault(c => c.Type == "scope")?.Value;

                }
                await _next(context);
            }
        }
    }

    public static class TokenRefreshMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenRefreshMiddleware(this IApplicationBuilder builder, string clientId, string clientSecret, string tokenEndpoint)
        {
            return builder.UseMiddleware<TokenRefreshMiddleware>(clientId, clientSecret, tokenEndpoint);
        }
    }

}
