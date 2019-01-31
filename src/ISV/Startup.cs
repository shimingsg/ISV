using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISV.Data;
using ISV.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ISV
{
    public class Startup
    {
        //public ILogger _logger { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //_logger = loggerFactory.CreateLogger<GlobalFiltersLogger>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("Sqlite")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddRazorPagesOptions(options =>
                 {
                     //options.Conventions.AuthorizePage("/Categories");
                     //options.Conventions.AllowAnonymousToPage("/Issues");
                     //options.Conventions.AddPageRoute("/Issues/Index", ""); 
                 })
                 .AddRazorOptions(options =>
                 {
                     options.AreaPageViewLocationFormats.Add("/Pages/Partials/{0}.cshtml");
                 });

            //services.Configure<IISOptions>(iis =>
            //{
            //    iis.AuthenticationDisplayName = "Windows";
            //    iis.AutomaticAuthentication = true;
            //});

            //services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddSingleton<IGithubService, GithubService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //app.UseAuthentication();
            app.UseMvc();
        }
    }
}
