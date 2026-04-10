using System.Threading.Tasks;

namespace HostelManage.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<string> InitiatePaymentAsync(int bookingId);
        Task<bool> VerifyPaymentAsync(string pidx);
    }
}
