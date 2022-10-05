using MenuTech.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuTech
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
            services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });

            //services.AddDbContext<MenuTechContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("YourConnnectionString")));
            services.AddDbContext<MenuTechContext>(options => { });
            //services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: "GetCustomerAccBalance",
                //    pattern: "{controller=Store}/{action=GetCustomerAccBalance}/{id?}"
                //    );

                //endpoints.MapControllerRoute(
                //        name: "Pay",
                //        pattern: "{controller=Store}/{action=Pay}"
                //    );

                //endpoints.MapControllerRoute(
                //        name: "Refund",
                //        pattern: "{controller=Store}/{action=Refund}"
                //    );
                //endpoints.MapControllerRoute(
                //        name: "default",
                //        pattern: "{controller=Store}/{action=GetStoreAccBalance}"
                //    );
            });
        }
    }
}
