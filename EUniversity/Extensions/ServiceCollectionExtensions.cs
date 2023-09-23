using EUniversity.Core.Models;
using EUniversity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Reflection;

namespace EUniversity.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new()
				{
					Title = "E-University",
					Version = "v1"
				});

				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
			});

			return services;
		}

		public static IServiceCollection AddCustomizedIdentity(this IServiceCollection services)
		{
			services
				.AddIdentity<ApplicationUser, IdentityRole>(
				options =>
				{
					options.Password.RequireDigit = true;
					options.Password.RequireLowercase = true;
					options.Password.RequireUppercase = true;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequiredLength = 8;
					options.Password.RequiredUniqueChars = 3;

					options.User.RequireUniqueEmail = true;
				})
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services
				.AddIdentityCore<ApplicationUser>(o =>
			{
				o.Stores.MaxLengthForKeys = 128;
			})
				.AddDefaultUI()
				.AddDefaultTokenProviders();

			services.AddIdentityServer()
				.AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

			services.AddAuthentication()
				.AddJwtBearer();

			services.ConfigureApplicationCookie(options =>
			{
				// Return 401 when user is not authrorized
				options.Events.OnRedirectToLogin = context =>
				{
					context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					return Task.CompletedTask;
				};
				// Return 403 when user don't have access permission
				options.Events.OnRedirectToAccessDenied = context =>
				{
					context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
					return Task.CompletedTask;
				};
			});

			return services;
		}
	}
}
