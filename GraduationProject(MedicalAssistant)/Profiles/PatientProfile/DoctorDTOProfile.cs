using AutoMapper;
using Models;
using Models.DTOs;


namespace GraduationProject_MedicalAssistant_.Profiles.PatientProfile
   
{
    public class DoctorDTOProfile:Profile
    {
        public DoctorDTOProfile()
        {
            CreateMap<Doctor, DoctorDTO>()
                            .ForMember(dest => dest.ClincNumbers,
                                opt => opt.MapFrom(src =>
                                    src.Clinics
                                       .SelectMany(c => c.phones)
                                       .Select(p => p.Phone)
                                       .ToList()
                                )).ForMember(dest => dest.Age, src => src.MapFrom(opt => DateTime.Today.Year -opt.BD.Year
                                )).ForMember(dest => dest.RattingAverage, opt => opt.MapFrom(src =>
                                     src.Ratings.Any() ? src.Ratings.Average(r => (int)r.Stars) : 0));




            ;
        }

       
    }
}
