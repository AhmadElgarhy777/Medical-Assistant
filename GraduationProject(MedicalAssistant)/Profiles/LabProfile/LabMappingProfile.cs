using AutoMapper;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.LabProfile
{
    public class LabMappingProfile : Profile
    {
        public LabMappingProfile()
        {
            CreateMap<Lab, LabDto>();
            CreateMap<MedicalTest, LabTestDto>();
            CreateMap<LabBooking, LabBookingDto>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FullName : "Unknown"))
                .ForMember(dest => dest.PatientPhone, opt => opt.MapFrom(src => "Unknown")) // Needs patient phone logic if available
                .ForMember(dest => dest.VisitType, opt => opt.MapFrom(src => src.VisitType.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<LabBookingItem, LabBookingItemDto>()
                .ForMember(dest => dest.TestName, opt => opt.MapFrom(src => src.MedicalTest != null ? src.MedicalTest.Name : ""))
                .ForMember(dest => dest.ResultStatus, opt => opt.MapFrom(src => src.Result != null ? src.Result.Status.ToString() : "NotReady"))
                .ForMember(dest => dest.ResultFileUrl, opt => opt.MapFrom(src => src.Result != null ? src.Result.ResultFileUrl : null));
        }
    }
}
