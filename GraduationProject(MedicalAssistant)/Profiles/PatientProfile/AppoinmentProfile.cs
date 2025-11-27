using AutoMapper;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.PatientProfile
{
    public class AppoinmentProfile:Profile
    {
        public AppoinmentProfile()
        {
            CreateMap<Appointment, AppointmentDTO>()

                .ForMember(s => s.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName));
        }

    }
}
