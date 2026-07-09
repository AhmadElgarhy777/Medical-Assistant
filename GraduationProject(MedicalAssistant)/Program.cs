using DataAccess;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.AiFeature.AnalyzeBrainTumorFeature.Validator;
using Features.AiFeature.ChestRayClassifcation;
using Features.AiFeature.SkinCancerClassification;
using Features.AiService;
using Features;
using Features.AuthenticationFeature.Validation;
using Features.NotifecationService;
using Features.PharmacyFeature;
using FluentValidation;
using GraduationProject_MedicalAssistant_.Extentions;
using GraduationProject_MedicalAssistant_.Hubs;
using InfrastructureExtension;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Models;
using Services.EmailServices;
using Services.PaymentServices;
using Services.TwilioProviderServices.WhatsUp;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GraduationProject_MedicalAssistant_
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging().LogTo(Console.WriteLine));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>
                (
                    options =>
                    {
                        options.User.AllowedUserNameCharacters = null;
                        options.User.RequireUniqueEmail = true;
                    }

                )
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders()
               .AddUserValidator<AllowDuplicateUserNameValidator<ApplicationUser>>();

            builder.Services.AddValidatorsFromAssemblyContaining<ResetPasswordByOtpDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<SkinCancerValidatorCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<AnalyzeBrainTumorCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ChestRayValidatorCommandValidator>();



            builder.Services.RemoveAll<IUserValidator<ApplicationUser>>();

            builder.Services.AddScoped<IUserValidator<ApplicationUser>,
                                       AllowDuplicateUserNameValidator<ApplicationUser>>();

            //builder.Services.Configure<HuggingFaceSettings>(
            //    builder.Configuration.GetSection("HuggingFace"));


            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            builder.Services.Configure<EmailSenderModel>(builder.Configuration.GetSection("EmailSenderModel"));
            builder.Services.Configure<TwilioSettingsModel>(builder.Configuration.GetSection("Twilio"));
            builder.Services.AddApiServices();
            builder.Services.AddSwaggerServices();
            builder.Services.CustomJwtServices(builder.Configuration);
            builder.Services.AddSwaggerAuth();
            builder.Services.AddCorsExtention();
            builder.Services.AddSignalR();

            // ✅ Pharmacy Services
            builder.Services.AddScoped<IPharmacyRepository, PharmacyRepository>();
            builder.Services.AddScoped<IPharmacyService, PharmacyService>();

            // ✅ Order Services
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();



            // ✅ Chat Services
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IConversationRepository, ConversationRepository>();


            builder.Services.AddHttpClient<IPaymentService, PaymentService>();
            //builder.Services.AddSignalR();
            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            var dbContext = service.GetRequiredService<ApplicationDbContext>();
            var FactoryLogger = service.GetRequiredService<ILoggerFactory>();
            var logger = FactoryLogger.CreateLogger<Program>();
            try
            {
                dbContext.Database.Migrate();
                await SuperAdminSeeder.SeedSuperAdminAsync(app.Services);
                //DataSedding.SpecilzationSeed(dbContext, logger);
            }
            catch (Exception ex)
            {
                FactoryLogger.CreateLogger<Program>().LogError(ex, "this is migration error ");
            }
            //app.MapHub<ChatHub>("/chat");
            app.MapHub<NotificationHub>("/notificationHub");
            app.AddSwaggerServiceMiddleWare();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}