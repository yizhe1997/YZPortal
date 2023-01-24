namespace YZPortal.Worker.Infrastructure.RazorLight
{
    public static class StartupExtensions
    {
        public static void AddEmailTemplates(this IServiceCollection services)
        {
            //services.AddTransient<IRazorLightEngine>(s =>
            //{
            //    return new RazorLightEngineBuilder()
            //        //.UseEmbeddedResourcesProject(new System.Reflection.Assembly)
            //        .UseFileSystemProject(Environment.CurrentDirectory)
            //        .UseMemoryCachingProvider()
            //        .Build();
            //    return new RazorLightEngineBuilder()
            //    .UseEmbeddedResourcesProject(typeof(Program))
            //    .UseMemoryCachingProvider()
            //    .Build();
            //});
        }
    }
}
