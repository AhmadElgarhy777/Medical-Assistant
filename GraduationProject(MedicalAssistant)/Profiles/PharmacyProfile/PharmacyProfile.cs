using AutoMapper;
using Models;
using Models.DTOs;
using Models.DTOs.RegistertionDTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.PharmacyProfile
{
    public class PharmacyProfile:Profile
    {
        public PharmacyProfile()
        {
            CreateMap<PrescriptionRequestDto, PrescriptionRequest>().ReverseMap();
               ;
        }
    }
}
