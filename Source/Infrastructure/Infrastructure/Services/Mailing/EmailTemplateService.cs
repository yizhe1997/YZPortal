using Application.Interfaces.Services.Mailing;

namespace Infrastructure.Services.Mailing
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel)
        {
            throw new NotImplementedException();
        }
    }
}
