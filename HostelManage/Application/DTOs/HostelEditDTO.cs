using System.ComponentModel.DataAnnotations;

namespace HostelManage.Application.DTOs
{
    public class HostelEditDTO
    {
        [StringLength(100)]
        public string HostelName { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public IFormFile HostelImage { get; set; }

        [Range(1, 4)]
        public int NumberOfRoomTypes { get; set; }

        public string? RoomType1 { get; set; }
        public decimal? RateType1 { get; set; }

        public string? RoomType2 { get; set; }
        public decimal? RateType2 { get; set; }

        public string? RoomType3 { get; set; }
        public decimal? RateType3 { get; set; }

        public string? RoomType4 { get; set; }
        public decimal? RateType4 { get; set; }
    }
}
