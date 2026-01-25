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
using InfrastructureExtension.ImageServices;
using InfrastructureExtension;
using Microsoft.OpenApi.Models;


namespace GraduationProject_MedicalAssistant_
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            
            builder.Services.AddDependancyInjectionScoped();

            builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(typeof(GetAllDoctorsSearchHandler).Assembly));
            builder.Services.AddAutoMapper(a=>a.AddProfile(typeof(AutoMaperProfile)),Assembly.GetExecutingAssembly());

            builder.Services.AddControllers().AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;

                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date"
                });
            });

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
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}
