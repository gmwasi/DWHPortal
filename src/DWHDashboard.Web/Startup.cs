using AngularASPNETCore2WebApiAuth.Extensions;
using AutoMapper;
using DWHDashboard.DashboardData.Data;
using DWHDashboard.ProfileManagement.Core.Interfaces;
using DWHDashboard.ProfileManagement.Core.Model;
using DWHDashboard.ProfileManagement.Infrastructure.Data;
using DWHDashboard.ProfileManagement.Infrastructure.Repository;
using DWHDashboard.Web.Auth;
using DWHDashboard.Web.Custom;
using DWHDashboard.Web.Helpers;
using DWHDashboard.Web.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using DWHDashboard.DashboardData.Repository;
using DWHDashboard.DashboardData.Repository.Implementation;
using DWHDashboard.ProfileManagement.Core.Services;
using DWHDashboard.SharedKernel.Data;
using DWHDashboard.Web.Services;

namespace DWHDashboard.Web
{
    public class Startup
    {
        public static IConfiguration Configuration;
        public static IServiceProvider ServiceProvider;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            var assemblyNames = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            List<Assembly> assemblies = new List<Assembly>();
            foreach (var assemblyName in assemblyNames)
            {
                assemblies.Add(Assembly.Load(assemblyName));
            }

            services.AddMvc()
                .AddMvcOptions(o => o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()))
                .AddJsonOptions(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.ConfigureWritable<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            var dashboardConnectionString = Startup.Configuration["ConnectionStrings:DwhDashboardConnection"];
            var dataConnectionString = Startup.Configuration["ConnectionStrings:DwhDataConnection"];

            services.AddDbContext<DwhDashboardContext>(b => b.UseSqlServer(dashboardConnectionString, x => x.MigrationsAssembly(typeof(DwhDashboardContext).GetTypeInfo().Assembly.GetName().Name)));
            services.AddDbContext<DwhDataContext>(b => b.UseSqlServer(dataConnectionString, x => x.MigrationsAssembly(typeof(DwhDataContext).GetTypeInfo().Assembly.GetName().Name)));
            services.AddTransient<DwhDashboardContext>();
            services.AddTransient<DwhDataContext>();

            services.AddScoped<IImpersonatorRepository, ImpersonatorRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<ITabViewRepository, TabViewRepository>();
            services.AddScoped<ITabWorkbookRepository, TabWorkbookRepository>();
            services.AddScoped<ITempOrgRepository, TempOrgRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IViralLoadMatrixRepository, ViralLoadMatrixRepository>();
            services.AddScoped<IQueryRepository, QueryRepository>();
            services.AddScoped<IPartnerMechanismRepository, PartnerMechanismRepository>();
            services.AddScoped<IFacilityRepository, FacilityRepository>();
            services.AddScoped<IDatimNewlyEnrolledRepository, DatimNewlyEnrolledRepository>();
            services.AddScoped<IDatimNewlyEnrolledBaselineCd4Repository, DatimNewlyEnrolledBaselineCd4Repository>();

            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<ITabViewService, TabViewService>();

            //Less secure apps must be turned on on google settings

            services.AddTransient<IEmailSender, EmailSender>(i =>
                new EmailSender(
                    Configuration["EmailSettings:Host"],
                    Configuration.GetValue<int>("EmailSettings:Port"),
                    Configuration.GetValue<bool>("EmailSettings:Ssl"),
                    Configuration["EmailSettings:UserName"],
                    Configuration["EmailSettings:Password"]
                )
            );

            //Authorization code must be obtained from the google developer console
            /*services.AddTransient<IEmailSender, SecureGmailSender>(i =>
                new SecureGmailSender(
                    Configuration["EmailSettings:Host"],
                    Configuration.GetValue<int>("EmailSettings:Port"),
                    Configuration["EmailSettings:UserName"],
                    Configuration["EmailSettings:AuthorizationCode"]
                )
            );*/

            // jwt wire up
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            string secretKey = Startup.Configuration["JwtKey"];
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });

            // add identity
            var builder = services.AddIdentityCore<User>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<DwhDashboardContext>().AddDefaultTokenProviders();

            services.AddAutoMapper();
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            /*var container = new Container();
            container.Populate(services);
            ServiceProvider = container.GetInstance<IServiceProvider>();*/

            ServiceProvider = services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseDefaultFiles();
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

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            app.UseExceptionHandler(
                builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                            var error = context.Features.Get<IExceptionHandlerFeature>();
                            if (error != null)
                            {
                                context.Response.AddApplicationError(error.Error.Message);
                                await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                            }
                        });
                });
            Log.Debug(@"initializing Database...");

            EnsureMigrationOfContext<DwhDashboardContext>(serviceProvider);
            Log.Debug(@"initializing Database [Complete]");

            Log.Debug("DWH Dashboard started !");
        }

        public static void EnsureMigrationOfContext<T>(IServiceProvider app) where T : DwhBaseContext
        {
            var contextName = typeof(T).Name;
            Log.Debug($"initializing Database context: {contextName}");
            var context = app.GetService<T>();
            try
            {
                context.Database.Migrate();
                context.EnsureSeeded();
                Log.Debug($"initializing Database context: {contextName} [OK]");
            }
            catch (Exception e)
            {
                Log.Debug($"initializing Database context: {contextName} Error");
                Log.Debug($"{e}");
            }

        }
    }
}