using Microsoft.OpenApi.Models;

namespace GraduationProject_MedicalAssistant_.Extentions
{
    public static class SwaggerServices
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date"
                });
            });

            return services;
        }
        public static WebApplication AddSwaggerServiceMiddleWare(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
