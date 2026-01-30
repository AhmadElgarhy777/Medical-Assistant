using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GraduationProject_MedicalAssistant_.Extentions
{
    public static class AuthServices
    {
        public static IServiceCollection CustomJwtServices(this IServiceCollection services , IConfiguration configuration )
        {
            services.AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(option =>
                {
                    option.RequireHttpsMetadata = false;
                    option.SaveToken = true;
                    option.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWTAuth:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JWTAuth:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTAuth:SecretKey"]))
                    };
                });

            return services;
        }
    }
}
