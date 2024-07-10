using Application.Interfaces.Services.Mailing;
using RazorLight;
using System.Reflection;
using System.Text;

namespace Infrastructure.Services.Mailing
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public async Task<string> GenerateEmailTemplate<T>(string templateName, T mailTemplateModel)
        {
            string template = GetTemplate(templateName);

            RazorLightEngine engine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(Assembly.GetEntryAssembly())
            .Build();
            
            string htmlContent = await engine.CompileRenderStringAsync(
                templateName,  // cache key
                template,
                mailTemplateModel);

            return htmlContent;
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
