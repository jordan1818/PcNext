namespace Microsoft.AspNetCore.Hosting;

/// <summary>
/// <see cref="IWebHostBuilder"/> extension methods for the CLI HTTP host.
/// </summary>
public static class HttpHostWebHostBuilderExtensions
{
    public static IWebHostBuilder AddDefaultMvcServices(this IWebHostBuilder builder)
    {
        return builder.ConfigureServices(services =>
        {
            var mvcBuilder = services.AddControllers();
            mvcBuilder.AddControllersAsServices();
        });
    }

    public static IWebHostBuilder AddDefaultSwaggerServices(this IWebHostBuilder builder)
    {
        return builder.ConfigureServices(services =>
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        });
    }
}
