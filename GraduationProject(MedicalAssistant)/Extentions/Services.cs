using DataAccess;
using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using Features.PatientFeature.Handler;
using GraduationProject_MedicalAssistant_.Profiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Services.ImageServices;
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
            services.AddScoped<IAppointmentRepositry, AppointmentRepositry>();
            services.AddScoped<IChatMessageRepositry, ChatMessageRepositry>();
            services.AddScoped<IChatRepositry, ChatRepositry>();
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




           services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllDoctorsSearchHandler).Assembly));
           services.AddAutoMapper(a => a.AddProfile(typeof(AutoMaperProfile)), Assembly.GetExecutingAssembly());
           services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }


    }
}
