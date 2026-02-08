using AutoMapper;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.PatientProfile
{
    public class PatientPhoneProfile:Profile
    {
        public PatientPhoneProfile()
        {
            CreateMap<PatientPhone, PatientPhonesDTO>().ReverseMap();   
        }
    }
}
