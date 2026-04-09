namespace HostelManage.Application.DTOs
{
    public class UserEditDTO
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Contact { get; set; }
        public IFormFile? Image { get; set; }
    }
}
