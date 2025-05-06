using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactManager.CORE.Domain.Entities.IdentityEntites;
using ContactManager.CORE.Services.IdentityServices;
using ContactManager.CORE.Services.PersonServices;
using ContactManager.CORE.Services.PersonsServices;
using ContactManager.CORE.Servicies.PersonServices;
using ContactManager.CORE.ServiciesContracts.IContryServices;
using ContactManager.CORE.ServiciesContracts.IIdentityServices;
using ContactManager.CORE.ServiciesContracts.IPersonServices;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using RepositoryContract;

namespace ContactManager.CORE
{
    public static  class IOCServicesExtension
    {
        public static IServiceCollection serviceDescriptors(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddControllersWithViews(options =>
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())

            );
            // Add services to the container.
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();

            services.AddScoped<ICountryGetter, ConuntryGetter>();
            services.AddScoped<ICountryAdder, CountryAdder>();
            

            services.AddScoped<IPersonGetter, PersonGetter>();
            services.AddScoped<IPersonsFIleGetter, PersonsFIleGetter>();

            services.AddScoped<IPersonAdder, PersonAdder>();
            services.AddScoped<IPersonDeleter, PersonDeleter>();
            services.AddScoped<IPersonUpdater, PersonUpdater>();
            services.AddScoped<IPersonSorter, PersonSorter>();

            services.AddSingleton<ICurrentApplicationUserService, CurrentApplicationUserService>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ContactManagerDbContext>()
            .AddDefaultTokenProviders()
            .AddUserStore<UserStore<ApplicationUser,ApplicationRole,ContactManagerDbContext,Guid>>()
            .AddRoleStore<RoleStore< ApplicationRole, ContactManagerDbContext, Guid>>();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });
            services.ConfigureApplicationCookie(options => { 
                options.LoginPath = "/Account/Login";
            });

            services.AddScoped<IUserRegisterService, UserRegisterService>();
            services.AddScoped<IUserLoginService, UserLoginService>();
            services.AddScoped<IUserLogoutService, UserLogoutService>();


            services.AddDbContext<ContactManagerDbContext>(
                options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                );
            return services;

        }
    }
}
