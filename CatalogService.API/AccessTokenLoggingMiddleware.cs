namespace CatalogService.API
{

    public class AccessTokenLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AccessTokenLoggingMiddleware> _logger;

        public AccessTokenLoggingMiddleware(RequestDelegate next, ILogger<AccessTokenLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var accessToken = context.Request.Headers["Authorization"];
            _logger.LogInformation($"Access Token: {accessToken}");


            await _next(context);
        }
    }

}
