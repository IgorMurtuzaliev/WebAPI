using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SCore.BLL.Infrastructure;
using SCore.BLL.Interfaces;
using SCore.BLL.Models;
using SCore.BLL.Services;
using SCore.DAL.EF;
using SCore.DAL.Interfaces;
using SCore.DAL.Repositories;
using SCore.Models;
using SCore.Models.Entities;

namespace SCore.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json").Build();
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connection));
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUnitOfWork, EFUnitOfWork>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("fully permissive", configurePolicy => configurePolicy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            });
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddTransient<IRepository<Product>, ProductRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRepository<User>, UserRepository>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IRepository<Order>, OrderRepository>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IFileManager, FileManager>();
            services.AddIdentity<User, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddMvc(options =>
            {

            });
            services.AddHttpContextAccessor();

            services.AddAuthentication(options =>
            {
                options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
            })
                .AddGoogle("Google", options =>
                {
                    options.ClientId = "405558759348-lv7doblutrpkqda42km1b1kd8eilcqu9.apps.googleusercontent.com";
                    options.ClientSecret = "c4pXS08zF9tzsKpMMyei3b-i";
                })
                .AddFacebook(options => {
                    options.AppId = "2895392233805084";
                    options.AppSecret = "c43fdffef09ddf0436fc7f3eb66f18f2";
                })
                 .AddCookie(options =>
                 {
                     options.LoginPath = "/Account/LogIn";

                 });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.RequireHttpsMetadata = false;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                            ValidateIssuer = true,
                            ValidIssuer = AuthOptions.ISSUER,
                            ValidateAudience = true,
                            ValidAudience = AuthOptions.AUDIENCE,
                            ValidateLifetime = true,
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                       };
                   });

            services.AddMemoryCache();
            services.AddSession();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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
            app.UseCors("fully permissive");
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
