using AutoMapper;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.PatientProfile
{
    public class PresciptionItemProfile:Profile
    {
        public PresciptionItemProfile()
        {
            CreateMap<PrescriptionItem, PresciptionItemDTO>();
        }
    }
}
