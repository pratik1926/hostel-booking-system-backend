using HostelManage.Application.DTOs.Hostel;

namespace HostelManage.Application.Interfaces
{
    public interface IHostelRoomService
    {
        Task<object> UploadRoomImages(int hostelId, HostelRoomUploadDTO dto);
        Task<object> GetRoomsByHostelId(int hostelId);
        Task<string> DeleteRoom(int roomId);
        Task<string> UpdateAllRoomImages(int hostelId, HostelRoomUploadDTO dto);
    }
}