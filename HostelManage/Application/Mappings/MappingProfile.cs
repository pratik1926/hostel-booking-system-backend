
using AutoMapper;
using HostelManage.Models;
using HostelManage.Application.DTOs.Booking;


namespace HostelManage.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookingCreateDTO, Booking>();
            CreateMap<BookingResponseDTO, Booking>();

            CreateMap<BookingCreateDTO, BookingResponseDTO>();
        }
    }
}
