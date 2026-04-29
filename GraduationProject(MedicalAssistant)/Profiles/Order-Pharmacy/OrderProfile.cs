using AutoMapper;
using Models;
using Models.DTOs;

namespace GraduationProject_MedicalAssistant_.Profiles.Order_Pharmacy
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
             CreateMap<Order, OrderResultDto>()
                .ForMember(e => e.OrderId, d => d.MapFrom(s => s.ID))
                .ForMember(e => e.InvoiceId, d => d.MapFrom(s => s.Invoice.ID))
                .ForMember(e => e.InvoiceTotalAmount, d => d.MapFrom(s => s.Invoice.TotalAmount))
                .ForMember(e => e.PaymentStatus, d => d.MapFrom(s => s.Invoice.PaymentStatus))
                .ReverseMap();
             
            CreateMap<Pharmacy, PharmacyRowDTO>()
                .ForMember(e => e.Governorate, d => d.MapFrom(s => s.Governorate.ToString()))
                .ForMember(e => e.Status, d => d.MapFrom(s => s.Status.ToString()))
                .ReverseMap();

            CreateMap<Pharmacy, PharmacyDetailsDTO>()
                .ForMember(e => e.Governorate, d => d.MapFrom(s => s.Governorate.ToString()))
                .ForMember(e => e.Status, d => d.MapFrom(s => s.Status.ToString()))
                .ForMember(e => e.Gender, d => d.MapFrom(s => s.Gender.ToString()))
                .ForMember(e => e.BD, d => d.MapFrom(s => s.BD.ToString()))
                .ReverseMap();



        }
    }
}
