using DataAccess.Repositry;
using DataAccess.Repositry.IRepositry;
using InfrastructureExtension.ImageServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureExtension
{
    public static class DependancyInjectionServices
    {
        public static  IServiceCollection AddDependancyInjectionScoped(this IServiceCollection services)
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

            return services;
        }


    }
}
