using App.Models.AutoMapper;
using AutoMapper;
using DAL.Repositories;
using Data.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StockApp.Data;
using System;
using System.Text;

namespace StockApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration _config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Injection
            // Injection 
            //services.AddScoped<IDatabaseInitializer, DatabaseInitializer>(); 
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                  .AddEntityFrameworkStores<AppDbContext>()
                  .AddDefaultTokenProviders();

            // MSSQL server
            //services.AddDbContext<AppDbContext>(options =>
            //   options.UseSqlServer(_config["ConnectionStrings:DefaultConnection"]));
           
            // Add Postgress
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(_config["ConnectionStrings:DefaultConnection"]));

            var Issuer = _config["Jwt:Issuer"];
            var Audience = _config["Jwt:Audience"];

            var sigInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            // var creds = new SigningCredentials(sigInKey, SecurityAlgorithms.HmacSha256);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = Issuer,
                IssuerSigningKey = sigInKey,
                ValidAudience = Audience,
                RequireExpirationTime = false, // __::todo- change
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          .AddJwtBearer(opt =>
          {
              opt.RequireHttpsMetadata = false;
              opt.SaveToken = true;
              opt.TokenValidationParameters = tokenValidationParameters;
          });

            services.AddAuthorization(auth =>
            {
                #region register Policies
                auth.AddPolicy("Bearer", po => po.RequireAuthenticatedUser());
                 
                // Users 
                auth.AddPolicy("view-users", policy => policy.RequireClaim("view-users"));
                auth.AddPolicy("create-users", policy => 
                    policy.RequireClaim("create-users")
                    //.RequireRole("super-admin")
                    );
                auth.AddPolicy("update-users", policy => policy.RequireClaim("update-users"));
                auth.AddPolicy("delete-users", policy => policy.RequireClaim("delete-users"));

                // Roles 
                auth.AddPolicy("view-roles", policy => policy.RequireClaim("view-roles"));
                auth.AddPolicy("create-roles", policy => policy.RequireClaim("create-roles"));
                auth.AddPolicy("update-roles", policy => policy.RequireClaim("update-roles"));
                auth.AddPolicy("delete-roles", policy => policy.RequireClaim("delete-roles"));

                auth.AddPolicy("super-admin", policy => policy.RequireRole("super-admin"));
                #endregion
            }); 

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();

                config.Filters.Add(new AuthorizeFilter(policy));

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // Allow anything till prod
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .Build();
                });
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist/StockApp";
                //configuration.RootPath = "https://mhdaxif.github.io/StockApp";
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseCors("EnableCORS");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp/dist/StockApp";
                //spa.Options.SourcePath = "https://mhdaxif.github.io/StockApp";

                if (env.IsDevelopment())
                {
                    // spa.Options.StartupTimeout = new TimeSpan(0, 1, 0);
                    // spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200"); // Use this instead to use the angular cli server
                }
            });
        }
    }
}
