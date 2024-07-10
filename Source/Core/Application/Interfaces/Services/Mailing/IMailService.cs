using Application.Requests;

namespace Application.Interfaces.Services.Mailing
{
    public interface IMailService
    {
        Task SendAsync(CreateMailCommand request, CancellationToken token = default);
    }
}
