using AutoMapper;
using Models;
using Models.DTOs;
using Models.DTOs.RegistertionDTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.RegisterationProfile
{
    public class RegisterProfile:Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterDoctorDTO, Doctor>()
                .ForMember(d => d.Address, d => d.MapFrom(s => s.AddressInDetails))
                .ForMember(d => d.BD, d => d.MapFrom(s => s.BirthDate))
                .ForMember(d => d.FullName, d => d.MapFrom(s => $"{s.FName}_{s.MName}_{s.LName}"))
                ;

            CreateMap<RegisterNurseDTO, Nures>()
                .ForMember(d => d.Address, d => d.MapFrom(s => s.AddressInDetails))
                .ForMember(d => d.BD, d => d.MapFrom(s => s.BirthDate))
                .ForMember(d => d.FullName, d => d.MapFrom(s => $"{s.FName}_{s.MName}_{s.LName}"))
              
                ;

            CreateMap<RegisterPatientDTO, Patient>()
                .ForMember(d => d.Address, d => d.MapFrom(s => s.AddressInDetails))
                .ForMember(d => d.BD, d => d.MapFrom(s => s.BirthDate))
                .ForMember(d => d.FullName, d => d.MapFrom(s => $"{s.FName}_{s.MName}_{s.LName}"))
                .ForMember(d=>d.patientPhones,d=>d.MapFrom(s=>new List<PatientPhone>()
                {
                    new PatientPhone
                    {
                        Phone=s.PhoneNumber,
                    }
                     
                }))
                ;

            CreateMap<Specialization, SpecializationDTO>().ReverseMap();
                    

            
        }
    }
}
