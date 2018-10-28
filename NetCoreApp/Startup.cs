using System;
using AutoMapper;
using CoreApp.Application.Implementation;
using CoreApp.Application.Interfaces;
using CoreApp.Application.Singleton;
using CoreApp.Data.EF;
using CoreApp.Data.EF.Registration;
using CoreApp.Data.Entities;
using CoreApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreApp.Authorization;
using NetCoreApp.Helpers;
using NetCoreApp.Services;
using Newtonsoft.Json.Serialization;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace NetCoreApp
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
            //Change AppDbContext if you use custom DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), o=>o.MigrationsAssembly("CoreApp.Data.EF")));

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.AddAutoMapper();

            // Add application services.
            //System
            
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();
            services.AddTransient<DbInitialize>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddMvc().AddJsonOptions(options=>options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, CustomClaimPrincipalFactory>();
            services.AddTransient(typeof(IRepository<,>), typeof(EFRepository<,>));
            services.AddTransient<IAuthorizationHandler, BaseResourceAuthorizationHandler>();
            //Auto Mapper
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));

            //
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IServiceRegistration, ServiceRegistration>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {
            logger.AddFile("Logs/App-{Date}.txt");
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            //Enable ASP.NET Identity
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name : "areas",
                    template : "{area:exists}/{controller=Login}/{action=Index}/{id?}"
                );
            });
        }
    }
}
