using AutoMapper;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.AdminProfile
{
    public class SuperAdminProfile:Profile
    {
        public SuperAdminProfile()
        {
            CreateMap<Pharmacy, PharmaciesDTO>()
                .ForMember(e => e.IsBanned, d => d.MapFrom(s => s.IsDeleted))
                .ForMember(e => e.BanCount, d => d.MapFrom(s => s.BanCount))
                .ReverseMap();
            CreateMap<Patient, PhatientsDTO>()
                .ForMember(e => e.IsBanned, d => d.MapFrom(s => s.IsDeleted))
                .ForMember(e => e.BanCount, d => d.MapFrom(s => s.BanCount))
                .ReverseMap();
            CreateMap<Doctor, DoctorsDTO>()
                .ForMember(e => e.SpecializationTitle, o => o.MapFrom(s => s.Specialization.Name))
                .ReverseMap();
        }
    }
}
