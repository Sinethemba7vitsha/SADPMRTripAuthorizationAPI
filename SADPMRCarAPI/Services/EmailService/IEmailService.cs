using SADPMRCarAPI.DTO.EmailDto;

namespace SADPMRCarAPI.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
        Task SendOtpCodeAsync(string email, string otpToken);
    }
}
