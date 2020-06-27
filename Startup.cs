using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Serana Wilger
/// 06/27/2020
/// Startup.cs
/// 
/// Startup file for the bot
/// </summary>

namespace sbwilger.BirdyBot
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            Bot bot = new Bot(serviceProvider);
            services.AddSingleton(bot);
        }

        public void Configure(IApplicationBuilder abb, IWebHostEnvironment env)
        {

        }
    }
}
