namespace CatalogService.API
{
    public class NoCacheMiddleware
    {
        private readonly RequestDelegate _next;

        public NoCacheMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Set no-cache headers for token-related endpoints

            context.Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate");
            context.Response.Headers.Add("Pragma", "no-cache");
            context.Response.Headers.Add("Expires", "-1");


            await _next(context);
        }
    }

}
