using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Serana Wilger
/// 06/18/2020
/// Program.cs
/// 
/// This is the main class which runs the bot
/// </summary>

namespace sbwilger.BirdyBot
{
    class Program
    {
        //instantiates and runs the bot
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
