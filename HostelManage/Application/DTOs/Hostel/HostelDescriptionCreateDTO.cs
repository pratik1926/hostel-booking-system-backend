namespace HostelManage.Application.DTOs.Hostel
{
    public class HostelDescriptionCreateDTO
    {
        public int HostelID { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int RoomType1Count { get; set; }
        public int RoomType2Count { get; set; }
        public int RoomType3Count { get; set; }
        public int RoomType4Count { get; set; }
    }
}
