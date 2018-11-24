using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace REST.MVC
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
