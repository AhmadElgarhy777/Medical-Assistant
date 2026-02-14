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
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            return services;
        }

    }
}
