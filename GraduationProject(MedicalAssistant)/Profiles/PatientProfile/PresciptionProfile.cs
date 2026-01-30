using AutoMapper;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.PatientProfile
{
    public class PresciptionProfile:Profile
    {
        public PresciptionProfile()
        {
            CreateMap<Prescription, PresciptionDTO>()
                .ForMember(e => e.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName));



        }
    }
}
