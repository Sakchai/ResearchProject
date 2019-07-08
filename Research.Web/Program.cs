using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Research
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseIISIntegration()
                   //.UseUrls("http://*:80/")
                   .UseStartup<Startup>();
    }
}
