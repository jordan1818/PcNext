namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// <see cref="IApplicationBuilder"/> extension methods for the CLI HTTP host.
/// </summary>
public static class HttpHostApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the default Swagger HTTP pipeline handlers.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <returns>The <paramref name="app"/> instance for chaining.</returns>
    public static IApplicationBuilder UseDefaultSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(o =>
        {
            o.SwaggerEndpoint("/swagger/v1/swagger.json", "API");

            // Set Swagger UI at apps root
            o.RoutePrefix = string.Empty; 
        });

        return app;
    }

    /// <summary>
    /// Adds the default MVC HTTP pipeline handlers.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
    /// <returns>The <paramref name="app"/> instance for chaining.</returns>
    public static IApplicationBuilder UseDefaultMvc(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(e => e.MapControllers());

        return app;
    }
}
