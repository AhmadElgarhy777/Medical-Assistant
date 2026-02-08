using Models.DTOs;
using Models;
using AutoMapper;

namespace GraduationProject_MedicalAssistant_.Profiles.AdminProfile
{
    public class NurseProfile:Profile
    {
        public NurseProfile()
        {
            CreateMap<Nures, NurseRowDTO>()
               .ForMember(e => e.Gender, d => d.MapFrom(s => s.Gender.ToString()))
               .ForMember(e => e.Age, d => d.MapFrom(s => DateTime.UtcNow.Year - s.BD.Year))
               .ReverseMap();

            CreateMap<Nures, NurseDetailseDTO>()
                .ForMember(e => e.Gender, d => d.MapFrom(s => s.Gender.ToString()))
                .ForMember(e => e.Governorate, d => d.MapFrom(s => s.Governorate.ToString()))
                .ForMember(e => e.Age, d => d.MapFrom(s => DateTime.UtcNow.Year - s.BD.Year))
                .ReverseMap();
        }
    }
}
