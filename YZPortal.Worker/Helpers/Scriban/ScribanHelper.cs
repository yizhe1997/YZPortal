using Scriban;

namespace YZPortal.Worker.Helpers.Scriban
{
    public static class ScribanHelper
    {
        public static Template? ParseTemplate(string templateName)
        {
            using (StreamReader reader = File.OpenText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templateName)))
            {
                string fileContent = reader.ReadToEnd();
                if (fileContent != null && fileContent != "")
                {
                    return Template.Parse(fileContent);
                }
            }

            return null;
        }
    }
}
