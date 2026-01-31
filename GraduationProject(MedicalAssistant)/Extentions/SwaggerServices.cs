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

        public static IServiceCollection AddSwaggerAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(option=>
            {
                option.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Medical Assistant System",
                });

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "Jwt",
                    In = ParameterLocation.Header
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                             Reference =new OpenApiReference()
                             {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                             },
                             Name="Bearer",
                             In = ParameterLocation.Header
                        },

                        new List<string>()
                    }
                });


            });
            return services;
        }

    }
}
