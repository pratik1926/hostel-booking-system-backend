namespace HostelManage.Application.DTOs
{
    public class FeedbackCreateDTO
    {
        public int HostelID { get; set; }   
        public int UserID { get; set; }
        public int Rating { get; set; }
        public string? Comments { get; set; }
    }
}
