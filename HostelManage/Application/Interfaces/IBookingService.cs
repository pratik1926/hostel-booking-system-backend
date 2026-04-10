using HostelManage.Models;
using HostelManage.Application.DTOs.Booking;
public interface IBookingService
{
    Task<int> GetBookingCount();
    Task<BookingResponseDTO> AddBooking(BookingCreateDTO dto);
    Task<IEnumerable<object>> GetBookingsByUserId(int userId);
    Task<IEnumerable<object>> GetBookingsByHostelId(int hostelId);
    Task UpdateBookingStatus(int id, int status);
    Task DeleteBooking(int id);
    Task<Booking> GetBookingById(int id);
    Task<IEnumerable<Booking>> GetAllBookings();
    Task<object> GetMyProfile(int userId);
    Task<string> BookRoom(int bookingId);
    Task MarkBookingAsPaid(int bookingId);
    

}