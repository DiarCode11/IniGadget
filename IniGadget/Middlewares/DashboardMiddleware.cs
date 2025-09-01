namespace IniGadget.Middlewares
{
    public class DashboardMiddleware
    {
        private readonly RequestDelegate _next;

        public DashboardMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/dashboard"))
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    context.Response.Redirect("/login");
                    return;
                }

                if (!context.User.IsInRole("Admin"))
                {
                    context.Response.StatusCode = 403; // Forbidden
                    await context.Response.WriteAsync("Access Denied!");
                    return;
                }
            }

            await _next(context);
        }
    }
}
