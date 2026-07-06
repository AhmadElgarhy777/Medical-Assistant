using DataAccess;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using DataAccess.UnitOfWork;
using Features.AiFeature.AnalyzeBrainTumorFeature;
using Features.AiFeature.CBCBloodTest;
using Features.AiFeature.ChestRayClassifcation;
using Features.AiFeature.SharedMethod;
using Features.AiFeature.SkinCancerClassification;
using Features.PatientFeature.Handler;
using GraduationProject_MedicalAssistant_.Profiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Services.EmailServices;
using Services.FileServices;
using Services.ImageServices;
using Services.OTPConfirmServices;
using Services.TwilioProviderServices.WhatsUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureExtension
{
    public static class Services
    {
        public static  IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddScoped<IAiReportRepositry, AiReportRepositry>();
            services.AddScoped<IAiReportImageRepositry, AiReportImageRepositry>();
            services.AddScoped<IPatientMedicalScanRepositry, PatientMedicalScanRepositry>();
            services.AddScoped<IAppointmentRepositry, AppointmentRepositry>();
            services.AddScoped<IClinicPhoneRepositry, ClinicPhoneRepositry>();
            services.AddScoped<IClinicRepositry, ClinicRepositry>();
            services.AddScoped<IDoctorAvaliableTimeRepositry, DoctorAvaliableTimeRepositry>();
            services.AddScoped<IDoctorPatientRepositry, DoctorPatientRepositry>();
            services.AddScoped<IDoctorRepositry, DoctorRepositry>();
            services.AddScoped<INuresRepositry, NurseRepositry>();
            services.AddScoped<IPatientPhoneRepositry, PatientPhoneRepositry>();
            services.AddScoped<IPatientRepositry, PatientRepositry>();
            services.AddScoped<IPresciptionItemRepositry, PrescriptionItemRepositry>();
            services.AddScoped<IPresciptionRepositry, PrescriptionRepositry>();
            services.AddScoped<IRatingRepositry, RatingRepositry>();
            services.AddScoped<ISpecilizationRepositry, SpecilizationRepositry>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IRefreshTokenRepositry, RefreshTokenRepositry>();
            services.AddScoped<IEmailServices, EmailServices>();
            services.AddScoped<IOTPConfirmEmailService, OTPConfirmEmailService>();
            services.AddScoped<IConversationPaticipantsRepositry, ConversationParticiPantsRepositry>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IPharmacyRepository, PharmacyRepository>();
            services.AddScoped<ICommentRepositry, CommentRepositry>();
            services.AddScoped<IAdminRepositry, AdminRepositry>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISmsService, TwilioWhatsAppService>();
            services.AddScoped<IPharmacyReposities, PharmacyReposities>();
            services.AddScoped<INurseServicesRepositry, NurseServiceRepositry>();
            services.AddScoped<INursingServicesRepositry, NurseingServiceRepositry>();
            services.AddHttpClient<IBrainTumorAIClient, BrainTumorAIClient>();
            services.AddHttpClient<ISkinCancerClassificationAIClient, SkinCancerClassificationAIClient>();
            services.AddHttpClient<IChestRayClassifcationAiClient, ChestRayClassifcationAiClient>();
            services.AddHttpClient<ICBCBloodTestAiClient, CBCBloodTestAiClient>();
            services.AddScoped<IAnalyzeImage, AnalyzeImage>();
            services.AddScoped<IAiAnalysisOrchestrator, AiAnalysisOrchestrator>();


            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ILabRepository, LabRepository>();

            services.AddMemoryCache();


           services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllDoctorsSearchHandler).Assembly));
           services.AddAutoMapper(a => a.AddProfile(typeof(AutoMaperProfile)), Assembly.GetExecutingAssembly());

          

           

            return services;
        }


    }
}
