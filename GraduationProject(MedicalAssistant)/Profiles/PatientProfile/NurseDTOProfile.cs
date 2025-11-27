using AutoMapper;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.PatientProfile
{
    public class NurseDTOProfile:Profile
    {
        public NurseDTOProfile()
        {
            CreateMap<Nures, NurseDTO>()
                
                .ForMember(dest=>dest.Age,opt=>opt.MapFrom(src=>DateTime.Now.Year - src.BD.Year))
                .ForMember(dest=>dest.RattingAverage, opt=>opt.MapFrom(src=>src.RattingAverage))
                
                
                
                
                
                ;
        }
    }
}
