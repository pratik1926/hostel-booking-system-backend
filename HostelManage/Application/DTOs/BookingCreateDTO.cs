namespace HostelManage.Application.DTOs
{
    public class BookingCreateDTO
    {
        public int UserId {  get; set; }
        public int HostelId {  get; set; }
        public string RoomType {  get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}
