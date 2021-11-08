using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PetProjectStore.Api.Configurations.Identity;
using PetProjectStore.Api.Interfaces;
using PetProjectStore.Api.Mapper.Profiles;
using PetProjectStore.Api.Services;
using PetProjectStore.DAL.Entities;
using PetProjectStore.DAL.EntityFramework;
using System;
using System.Linq;
using System.Security.Claims;

namespace PetProjectStore.Api
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
            services.AddControllers();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddSingleton(Configuration.GetSection("DefaultUserConfiguration").Get<DefaultUserConfiguration>());

            services.AddSingleton(new MapperConfiguration(cfg =>
                {
                    cfg.AddProfiles(new Profile[]
                    {
                        new EntityMapperProfile()
                    });
                })
                .CreateMapper());

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("all",
                    policy => { policy.RequireClaim(ClaimTypes.Role, UserRole.Admin.ToString(), UserRole.User.ToString()); });

                opts.AddPolicy(nameof(UserRole.Admin),
                    policy => { policy.RequireClaim(ClaimTypes.Role, UserRole.Admin.ToString()); });

                opts.AddPolicy(nameof(UserRole.User),
                    policy => { policy.RequireClaim(ClaimTypes.Role, UserRole.User.ToString()); });
            });

            ConfigureDb(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PetProjectStore.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetProjectStore.Api v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ExecuteDbMigrations(app);
            ExecuteIdentityDbMigrations(app);
            SeedIdentityRoles(app);
        }

        private void ConfigureDb(IServiceCollection services)
        {
            services.AddDbContextPool<StoreContext>(opt =>
                opt.UseSqlServer(
                    Configuration.GetConnectionString("StoreDbConnection")));

            services.AddDbContextPool<IdentityContext>(opt =>
                opt.UseSqlServer(
                    Configuration.GetConnectionString("IdentityDbConnection")));

            //services.AddSingleton<UserManager<User>>();

            services.AddIdentityCore<User>(opts =>
            {
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireNonAlphanumeric = true;
                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = null;
            })
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<IdentityContext>();
        }

        private void SeedIdentityRoles(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var roleName in Enum.GetNames(typeof(UserRole)))
            {
                if (!roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            if (userManager.Users.Any())
                return;

            var configuration = serviceProvider.GetRequiredService<DefaultUserConfiguration>();

            User user = new User
            {
                UserName = "admin",
                Email = configuration.Email,
                EmailConfirmed = false
            };

            var createResult = userManager.CreateAsync(user, configuration.Password).GetAwaiter().GetResult();

            userManager.AddToRoleAsync(user, UserRole.Admin.ToString()).GetAwaiter().GetResult();
        }

        private void ExecuteIdentityDbMigrations(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

            var storeContext = serviceScope.ServiceProvider.GetRequiredService<IdentityContext>();

            storeContext.Database.Migrate();
        }

        private void ExecuteDbMigrations(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

            var storeContext = serviceScope.ServiceProvider.GetRequiredService<StoreContext>();

            storeContext.Database.Migrate();
        }
    }
}
