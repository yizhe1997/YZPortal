namespace Application.Interfaces.Services.Mailing
{
    public interface IEmailTemplateService
    {
        string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel);
    }
}
