namespace HostelManage.Application.DTOs
{
    public class FeedbackResponseDTO
    {
        public int HostelID { get; set; }
        public int UserID { get; set; }
        public int Rating { get; set; }
        public string? Comments { get; set; }
        public DateTime FeedbackDate { get; set; }
        public string Username { get; set; }
    }
}
