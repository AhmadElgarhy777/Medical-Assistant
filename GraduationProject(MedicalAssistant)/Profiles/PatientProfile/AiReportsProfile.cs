using AutoMapper;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.PatientProfile
{
    public class AiReportsProfile:Profile
    {
        public AiReportsProfile()
        {
            CreateMap<AiReport, AiReportDTO>()
                .ForMember(e=>e.DoctorName,opt=>opt.MapFrom(src=>src.Doctor.FullName))
                .ForMember(e => e.DoctorSpeclization, opt => opt.MapFrom(src => src.Doctor.Specialization.Name));

        }
    }
}
