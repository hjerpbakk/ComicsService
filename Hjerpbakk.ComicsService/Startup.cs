using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hjerpbakk.ComicsService;
using Hjerpbakk.ComicsService.Cache;
using Hjerpbakk.ComicsService.Clients;
using Hjerpbakk.ComicsService.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Hjerpbakk.ComicService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddMvc();

            var configuration = ReadConfig();
            services.AddSingleton<IReadOnlyAppConfiguration>(configuration);
            services.AddSingleton<ICache, Cache>();
            services.AddSingleton<IFeedReaderClient, FeedReaderClient>();
            services.AddSingleton<ComicsClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseStaticFiles();
        }

		static AppConfiguration ReadConfig()
		{
			return JsonConvert.DeserializeObject<AppConfiguration>(File.ReadAllText("config.json"));
		}
    }
}
