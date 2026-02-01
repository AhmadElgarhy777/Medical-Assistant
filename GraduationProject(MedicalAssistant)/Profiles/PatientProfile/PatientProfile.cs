using AutoMapper;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.PatientProfile
{
    public class PatientProfile:Profile
    {
        public PatientProfile()
        {
            CreateMap<Patient,PatientPeofileDTO>()
                .ForMember(p=>p.Governorate,p=>p.MapFrom(s=>s.Governorate.ToString()))
                .ForMember(p=>p.Gender,p=>p.MapFrom(s=>s.Gender.ToString()))
                .ReverseMap();
        }
    }
}
