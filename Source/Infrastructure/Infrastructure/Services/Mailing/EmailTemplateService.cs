using Application.Interfaces.Services.Mailing;
using System.Text;

namespace Infrastructure.Services.Mailing
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel)
        {
            string template = GetTemplate(templateName);

            return template;
        }

        public static string GetTemplate(string templateName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string tmplFolder = Path.Combine(baseDirectory, "EmailTemplates");
            string filePath = Path.Combine(tmplFolder, $"{templateName}.cshtml");

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs, Encoding.Default);
            string mailText = sr.ReadToEnd();
            sr.Close();

            return mailText;
        }
    }
}
