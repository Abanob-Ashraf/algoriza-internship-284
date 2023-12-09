using VezeetaApi.Domain.Dtos;

namespace VezeetaApi.Domain.Services
{
    public interface ISendingEmailService
    {
        Task SendEmailAsync(MailMessageDTO mailMessageDTO);
    }
}
