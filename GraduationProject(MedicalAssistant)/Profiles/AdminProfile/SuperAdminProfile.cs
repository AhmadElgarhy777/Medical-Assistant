using AutoMapper;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.AdminProfile
{
    public class SuperAdminProfile:Profile
    {
        public SuperAdminProfile()
        {
            CreateMap<Pharmacy,PharmaciesDTO>().ReverseMap();
            CreateMap<Patient, PhatientsDTO>().ReverseMap();
            CreateMap<Doctor, DoctorsDTO>()
                .ForMember(e=>e.SpecializationTitle,o=>o.MapFrom(s=>s.Specialization.Name))
                .ReverseMap();
        }
    }
}
