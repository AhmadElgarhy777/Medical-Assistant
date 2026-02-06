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



            var app = builder.Build();

            using var scope  = app.Services.CreateScope();
            var service= scope.ServiceProvider;
            var dbContext=service.GetRequiredService<ApplicationDbContext>();
            var FactoryLogger = service.GetRequiredService<ILoggerFactory>();

            try
            {
                dbContext.Database.MigrateAsync();
                //DataSedding.SpecilzationSeed(dbContext);
            }
            catch (Exception ex)
            {
                FactoryLogger.CreateLogger<Program>().LogError(ex,"this is migration error ");

            }

            if (app.Environment.IsDevelopment())
            {
                app.AddSwaggerServiceMiddleWare();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}
