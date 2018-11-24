using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Authentication.Custom;
using API.Data.Entities;
using API.Data.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Config
{
    public class StartupConfig
    {
        public static IConfiguration Configuration;

        public StartupConfig(IConfiguration configuration) {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceCollection AddConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            string identityConnectionString = StartupConfig.Configuration["connectionStrings:LocalDbIdentity"];
            string migrationsAssembly = typeof(StartupConfig).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<UserDbContext>(options => {
                options.UseSqlServer(identityConnectionString, sql => {
                    sql.MigrationsAssembly("API.Data");
                });
            });

            services.AddIdentity<UserModel, IdentityRole>(options => {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Tokens.EmailConfirmationTokenProvider = "emailConfirm";
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredUniqueChars = 5;
                    options.Password.RequiredLength = 15;

                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<EmailConfirmationTokenProvider<UserModel>>("emailConfirm")
                .AddPasswordValidator<CustomPasswordValidator<UserModel>>();

            services.Configure<DataProtectionTokenProviderOptions>(options => {
                options.TokenLifespan = TimeSpan.FromHours(3);
            });
            services.Configure<EmailConfirmationTokenProviderOptions>(options => {
                options.TokenLifespan = TimeSpan.FromDays(2);
            });
            services.Configure<PasswordHasherOptions>(options => {
                options.IterationCount = 100000;
            });
            services.AddScoped<IUserStore<UserModel>, UserOnlyStore<UserModel, UserDbContext>>();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/api/login");

            services.Configure<RazorViewEngineOptions>(options => {
                // options.ViewLocationExpanders.Add(new ViewLocationExpander());
                // options.AreaViewLocationFormats.Clear();
                // options.AreaViewLocationFormats.Add("/API.Authentication/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
            });

            return services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // public void Configure(IApplicationBuilder app, IAntiforgery antiforgery, IHostingEnvironment env)
        // {
        //     if (env.IsDevelopment())
        //     {
        //         app.UseDeveloperExceptionPage();
        //     }

        //     app.UseAuthentication();


        //     app.UseMvc();
        // }
    }
}
