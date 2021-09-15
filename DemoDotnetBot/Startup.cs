using DemoDotnetBot.Models;
using DemoDotnetBot.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DemoDotnetBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            BotConfig = Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
        }

        public IConfiguration Configuration { get; }
        public BotConfiguration BotConfig { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<ConfigureWebhook>();

            services.AddHttpClient("tgwebhook")
                .AddTypedClient<ITelegramBotClient>(httpClient =>
                new TelegramBotClient(BotConfig.Token, httpClient));

            services.AddScoped<HandleUpdateService>();

            services.AddControllers()
                .AddNewtonsoftJson();
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                var token = BotConfig.Token;

                endpoints.MapControllerRoute(name: "tgwebhook",
                    pattern: $"bot/{token}",
                    new { controller = "Webhook", action = "Post" });

                endpoints.MapControllers();
            });
        }
    }
}
