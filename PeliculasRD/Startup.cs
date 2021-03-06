﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PeliculasRD.Models;
using PeliculasRD.Services.Interfaces;
using PeliculasRD.Services.Repositories;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace PeliculasRD
{
    public class Startup
    {
        IConfiguration Configuration;
        
        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            /*************Identity**************/

            services.AddDbContext<AppIdentityDbContext>(opts => {
                opts.UseSqlServer(Configuration["Data:PeliculasRD:ConnectionString"]);
            });

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            /*************Movies**************/
            services.AddDbContext<ApplicationDbContext>(opts => {
                opts.UseSqlServer(Configuration["Data:PeliculasRDMovies:ConnectionString"]);
            });

            services.AddTransient<IMovieRepository, MovieRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            //The wait method in the end is beacause the method CreateAdminAccount is Task
            AppIdentityDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
            SeedMovieData.SeedMovies(app, Configuration).Wait();
        }
    }
}
