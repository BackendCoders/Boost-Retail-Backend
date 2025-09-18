namespace SConnect.Middleware
{
    public class TokenAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _token;
        private readonly bool _enabled = true; // Set to false to disable token authentication

        public TokenAuthMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _token = config["ApiToken"];
            _enabled = bool.TryParse(config["TokenAuthEnabled"], out var enabled) && enabled;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var providedToken = context.Request.Headers["Authorization"].ToString();

            if (_enabled)
            {
                if (string.IsNullOrEmpty(providedToken) || providedToken != _token)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { error = "Unauthorized1" });
                    return;
                }
            }

            await _next(context);
        }
    }
}
