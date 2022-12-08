using Allup.DAL;
using Allup.Interfaces;
using Allup.Model;
using Allup.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson(options => 
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ILayoutService, LayoutService>();
            services.AddHttpContextAccessor();
            services.AddIdentity<AppUser, IdentityRole>(options =>
             {
                 //parol 8den yuxari
                 options.Password.RequiredLength = 8;
                 //gelen sifreden bunlari teleb edecek
                 options.Password.RequireDigit = true;
                 options.Password.RequireLowercase = true;
                 options.Password.RequireUppercase = true;
                 // parolda simvol olmalidir
                 //options.Password.RequireNonAlphanumeric = true;



                 //emailde eyni qeydiyat olsa error verir eyni olmaz.
                 options.User.RequireUniqueEmail = true;

                 //parolu 3 defeden cox sehf yigsa bloklanir nece dege versek.
                 options.Lockout.MaxFailedAccessAttempts = 3;
                 //3 defe sehf parol yigsa 90 saniye bloklanir
                 options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(90);
                 //bu yeni user qerydiyatdan kecse parol cixsa yadinnan 3 defen cox sehf yiga biler.
                 options.Lockout.AllowedForNewUsers = true;

             }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();


            //login olub olmadigini yoxluyur.
            app.UseAuthentication();
            //login olmusan neye icazen var ya yoxdu onu yoxluyur.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    
                    name: "areas",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"

                    );

                endpoints.MapControllerRoute(
                    name:"Default",
                    pattern:"{controller=Home}/{action=Index}/{Id?}"
                    );


            });
        }
    }
}
