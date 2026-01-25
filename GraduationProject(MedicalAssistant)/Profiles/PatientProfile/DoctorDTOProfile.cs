using AutoMapper;
using Models;
using Models.DTOs;


namespace GraduationProject_MedicalAssistant_.Profiles.PatientProfile
   
{
    public class DoctorDTOProfile : Profile
    {
        public DoctorDTOProfile()
        {
            CreateMap<Doctor, DoctorDTO>()
                .ForMember(e => e.ClincNumbers,
                    opt =>opt.MapFrom(src =>src.Clinics
                            .SelectMany(o => o.phones)
                            .Select(o => o.Phone)
                            .ToList()
                            ))
                .ForMember(e => e.Age, opt => opt.MapFrom(src => DateTime.Now.Year - src.BD.Year))
                .ForMember(e => e.RattingAverage, opt => opt.MapFrom(src => src.RattingAverage))
                ;





      



    }
}
  }

