using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Kero_Auth.Extensions;

public static class SwaggerConfiguration
{
    public static WebApplication AddSwaggerUi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.InjectStylesheet("/swagger-ui/SwaggerDark.css");
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });
        return app;
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options => 
        {
            options.SwaggerDoc("v1", new() 
            {
                Version = "v1",
                Title = "KeroAuth Api",
                Description = "KeroAuth is an API for managing authentication and authorization. The name 'Kero' is inspired by Kero, the guardian beast from CardCaptor Sakura, symbolizing the protection of user credentials stored in Firebase.",
                License = new OpenApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0"),
                }
            });
            var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
        });
    }
}
