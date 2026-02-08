using AutoMapper;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.AdminProfile
{
    public class DoctorProfile:Profile
    {
        public DoctorProfile()
        {
            CreateMap<Doctor,DoctorRowDTO>()
                .ForMember(e=>e.Specialization,d=>d.MapFrom(s=>s.Specialization.Name))
                .ForMember(e=>e.Gender,d=>d.MapFrom(s=>s.Gender.ToString()))
                .ForMember(e=>e.Age,d=>d.MapFrom(s=>DateTime.UtcNow.Year-s.BD.Year))
                .ReverseMap();

            CreateMap<Doctor, DoctorDetailsDTO>()
                .ForMember(e => e.Specialization, d => d.MapFrom(s => s.Specialization.Name))
                .ForMember(e => e.Gender, d => d.MapFrom(s => s.Gender.ToString()))
                .ForMember(e => e.Governorate, d => d.MapFrom(s => s.Governorate.ToString()))
                .ForMember(e => e.Age, d => d.MapFrom(s => DateTime.UtcNow.Year - s.BD.Year))
                .ReverseMap();
        }
    }
}
