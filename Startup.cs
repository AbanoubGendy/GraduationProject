using Demo.DAL.Models;
using Graduation.BLL;
using Graduation.BLL.Interfaces;
using Graduation.BLL.Repositories;
using Graduation.DAL.Data;
using Graduation.PL.Helpers;
using Graduation.PL.Servises.EmailServises;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduationProject
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
            services.AddControllersWithViews();

            services.AddTransient<IEmailSetting, EmailSetting>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<GraduationDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAutoMapper(o => o.AddProfile(new MappingProfiles()));


            services.AddIdentity<ApplicationUser, IdentityRole>(config =>  //To config inputs in user
            {
                config.User.RequireUniqueEmail = true;    //the configration
                config.Password.RequiredLength = 5;       //the configration

            }).AddEntityFrameworkStores<GraduationDbContext>() //to allowe the store services in packege 
            .AddDefaultTokenProviders(); // to generate to to some servisec ex reset password change email change phonenumber

            services.ConfigureApplicationCookie(config => //to configre the built in token (Coockies)
            {
                config.AccessDeniedPath = "/Home/Error";
                config.LoginPath = "/Account/LogIn";
                config.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Cover}/{id?}");
            });
        }
    }
}
