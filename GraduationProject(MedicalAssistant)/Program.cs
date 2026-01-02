using MediatR;
using DataAccess;
using DataAccess.Repositry.IRepositry;
using DataAccess.Repositry;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Reflection;
using Features.PatientFeature.Handler;

namespace GraduationProject_MedicalAssistant_
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

            #region ScopedServices

            builder.Services.AddScoped<IAiReportRepositry, AiReportRepositry>();
            builder.Services.AddScoped<IAppointmentRepositry, AppointmentRepositry>();
            builder.Services.AddScoped<IChatMessageRepositry, ChatMessageRepositry>();
            builder.Services.AddScoped<IChatRepositry, ChatRepositry>();
            builder.Services.AddScoped<IClinicPhoneRepositry, ClinicPhoneRepositry>();
            builder.Services.AddScoped<IClinicRepositry, ClinicRepositry>();
            builder.Services.AddScoped<IDoctorAvaliableTimeRepositry, DoctorAvaliableTimeRepositry>();
            builder.Services.AddScoped<IDoctorPatientRepositry, DoctorPatientRepositry>();
            builder.Services.AddScoped<IDoctorRepositry, DoctorRepositry>();
            builder.Services.AddScoped<INuresRepositry, NurseRepositry>();
            builder.Services.AddScoped<IPatientPhoneRepositry, PatientPhoneRepositry>();
            builder.Services.AddScoped<IPatientRepositry, PatientRepositry>();
            builder.Services.AddScoped<IPresciptionItemRepositry, PrescriptionItemRepositry>();
            builder.Services.AddScoped<IPresciptionRepositry, PrescriptionRepositry>();
            builder.Services.AddScoped<IRatingRepositry, RatingRepositry>();
            builder.Services.AddScoped<ISpecilizationRepositry, SpecilizationRepositry>();
            #endregion
            #region Services

            builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(typeof(GetAllDoctorsSearchHandler).Assembly));
            builder.Services.AddAutoMapper(
                           configration => { },
                           Assembly.GetExecutingAssembly()
                           );
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion

            var app = builder.Build();

            using var scope  = app.Services.CreateScope();
            var service= scope.ServiceProvider;
            var dbContext=service.GetRequiredService<ApplicationDbContext>();
            var FactoryLogger = service.GetRequiredService<ILoggerFactory>();

            try
            {
                dbContext.Database.MigrateAsync();
                DataSedding.SpecilzationSeed(dbContext);
            }
            catch (Exception ex)
            {
                FactoryLogger.CreateLogger<Program>().LogError(ex,"this is migration error ");

            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
