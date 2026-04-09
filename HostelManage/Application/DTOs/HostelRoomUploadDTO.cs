using Microsoft.AspNetCore.Http;

namespace HostelManage.Application.DTOs
{
    public class HostelRoomUploadDTO
    {
        public int HostelID { get; set; }

        public IFormFile? RoomImage1 { get; set; }
        public IFormFile? RoomImage2 { get; set; }
        public IFormFile? RoomImage3 { get; set; }
        public IFormFile? RoomImage4 { get; set; }
    }
}