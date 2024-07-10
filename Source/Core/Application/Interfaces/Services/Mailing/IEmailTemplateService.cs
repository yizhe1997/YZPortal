namespace Application.Interfaces.Services.Mailing
{
    public interface IEmailTemplateService
    {
        Task<string> GenerateEmailTemplate<T>(string templateName, T mailTemplateModel);
    }
}
