using AutoMapper;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.RadiologyProfile
{
    public class RadiologyMappingProfile : Profile
    {
        public RadiologyMappingProfile()
        {
            CreateMap<RadiologyCenter, RadiologyCenterDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            
            // Note: LabBooking is also used for Radiology Appointments per our reuse strategy.
            CreateMap<LabBooking, RadiologyAppointmentDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FullName : "Unknown"))
                .ForMember(dest => dest.PatientPhone, opt => opt.MapFrom(src => "Unknown"))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.ScanName, opt => opt.MapFrom(src => src.Items.FirstOrDefault() != null && src.Items.FirstOrDefault()!.RadiologyScan != null ? src.Items.FirstOrDefault()!.RadiologyScan!.Name : ""))
                .ForMember(dest => dest.ResultFileUrl, opt => opt.MapFrom(src => src.Items.FirstOrDefault() != null && src.Items.FirstOrDefault()!.RadiologyResult != null ? src.Items.FirstOrDefault()!.RadiologyResult!.ReportFileUrl : null))
                .ForMember(dest => dest.ImagesUrls, opt => opt.MapFrom(src => src.Items.FirstOrDefault() != null && src.Items.FirstOrDefault()!.RadiologyResult != null ? src.Items.FirstOrDefault()!.RadiologyResult!.ImagesUrls : null))
                .ForMember(dest => dest.DoctorNotes, opt => opt.MapFrom(src => src.Items.FirstOrDefault() != null && src.Items.FirstOrDefault()!.RadiologyResult != null ? src.Items.FirstOrDefault()!.RadiologyResult!.DoctorNotes : null));

            // Map MedicalTest/RadiologyScan? Wait, the user asked for RadiologyScans. 
            // In the DB context, there is a DbSet<RadiologyScan>.
            CreateMap<RadiologyScan, RadiologyScanOfferDto>()
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => "N/A")) // Add Duration property if not exists
                .ForMember(dest => dest.Preparation, opt => opt.MapFrom(src => "N/A")); // Add Preparation property if not exists
        }
    }
}
