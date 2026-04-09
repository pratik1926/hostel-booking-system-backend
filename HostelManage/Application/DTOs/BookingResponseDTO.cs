namespace HostelManage.Application.DTOs
{
    public class BookingResponseDTO
    {
        public int BookingID { get; set; }
        public int UserID { get; set; }
        public int HostelId { get; set; }
        public string RoomType { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Status { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
