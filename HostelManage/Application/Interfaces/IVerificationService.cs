namespace HostelManage.Application.Interfaces
{
    public interface IVerificationService
    {
        Task SendCodeAsync(string email);
        bool VerifyCode(string email, string code);

    }
}
