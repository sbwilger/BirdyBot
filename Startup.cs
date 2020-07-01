using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using sbwilger.DAL;
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
            services.AddDbContext<RPGContext>(options =>
            {
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=RPGContext;Trusted_Connection=true;MultipleActiveResultSets=true",
                    x => x.MigrationsAssembly("sbwilger.DAL.Migrations"));
            });

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            Bot bot = new Bot(serviceProvider);
            services.AddSingleton(bot);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
