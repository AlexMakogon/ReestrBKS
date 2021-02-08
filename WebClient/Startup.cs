using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReestrBKS.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReestrBKS.DataAccess.Interfaces;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

namespace ReestrBKS.WebClient
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

            string connectionName = (string)Configuration.GetValue(typeof(string), "ConnectionName");
            string connectionString = Configuration.GetConnectionString(connectionName);
            if (connectionName == "sql")
                services.AddDbContext<ReestrContext>(option => option.UseSqlServer(connectionString));
            else if (connectionName == "sqlite")
                services.AddDbContext<ReestrContext>(option => option.UseSqlite(connectionString));

            services.AddTransient<IAmountTypeRepository, AmountTypeRepository>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<IRepositoryStore, RepositoryStore>();
            services.AddTransient<IStreetRepository, StreetRepository>();
            services.AddTransient<ISubjectRepository, SubjectRepository>();
            services.AddTransient<ISubjectTypeRepository, SubjectTypeRepository>();

            services.AddHttpContextAccessor();
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 100000000;
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = 100000000; // if don't set default value is: 30 MB
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
