namespace GameInventoryApiStefanKobetich.Middleware;

// Middleware to check for API key in the header of incoming requests
public class Middleware(RequestDelegate next)
{
    // The API key is expected to be in the header "X-Api-Key" and must match "MIDTERM_KEY_123"
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext ctx)
    {
        // Skip API key check for Swagger endpoints (only really used in dev)
        // Not really important here as anything in swagger wont work, as I cant add the API key there
        // Leaving it in as swagger was required
        if (ctx.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(ctx);
            return;
        }

        if (!ctx.Request.Headers.TryGetValue("X-Api-Key", out var key) || key != "MIDTERM_KEY_123")
        {
            ctx.Response.StatusCode = 401;
            await ctx.Response.WriteAsJsonAsync(new { error = "Unauthorized", message = "Missing or invalid API key." });

            return;
        }
        
        await _next(ctx);
    }
}

// Extension method to add the middleware to the app pipeline (called in Program.cs)
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseApiKey(this IApplicationBuilder builder) =>
        builder.UseMiddleware<Middleware>();
}
