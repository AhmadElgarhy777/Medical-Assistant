using MediatR;
using DataAccess;
using DataAccess.Repositry.IRepositry;
using DataAccess.Repositry;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Reflection;
using Features.PatientFeature.Handler;
using Microsoft.AspNetCore.Identity;
using GraduationProject_MedicalAssistant_.Profiles;
using System.Text.Json.Serialization;
using InfrastructureExtension;
using GraduationProject_MedicalAssistant_.Extentions;
using Services.EmailServices;
using Microsoft.Extensions.Logging;
using Features.PharmacyFeature;
using DataAccess.Repositry;

namespace GraduationProject_MedicalAssistant_
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });
            builder.Services.Configure<EmailSenderModel>(builder.Configuration.GetSection("EmailSenderModel"));
            builder.Services.AddApiServices();
            builder.Services.AddSwaggerServices();
            builder.Services.CustomJwtServices(builder.Configuration);
            builder.Services.AddSwaggerAuth();
            builder.Services.AddCorsExtention();

            // ✅ Pharmacy Services
            builder.Services.AddScoped<IPharmacyRepository, PharmacyRepository>();
            builder.Services.AddScoped<IPharmacyService, PharmacyService>();

            // ✅ Order Services
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            var dbContext = service.GetRequiredService<ApplicationDbContext>();
            var FactoryLogger = service.GetRequiredService<ILoggerFactory>();
            var logger = FactoryLogger.CreateLogger<Program>();
            try
            {
                dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                FactoryLogger.CreateLogger<Program>().LogError(ex, "this is migration error ");
            }
            app.AddSwaggerServiceMiddleWare();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}