namespace GraduationProject_MedicalAssistant_.Extentions
{
    public static class CorsServices
    {
        public static IServiceCollection AddCorsExtention(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                   
                        policy.SetIsOriginAllowed(_ => true)
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                   
                });
                
            });
            return services;
        }

    }
}
