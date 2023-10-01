using EUniversity.Core.Models;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using EUniversity.Core.Validation;
using EUniversity.Infrastructure.Data;
using EUniversity.Infrastructure.Identity;
using EUniversity.Infrastructure.Services;
using FluentValidation;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Net;
using System.Reflection;
using System.Text.Json.Serialization;

namespace EUniversity.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddSwagger(this IServiceCollection services)
		{
			return services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new()
				{
					Title = "E-University",
					Version = "v1"
				});

				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
			});
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
					// Alphanumeric characters with dashes, underscores and periods
					options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
				})
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services
				.AddIdentityCore<ApplicationUser>(o =>
			{
				o.Stores.MaxLengthForKeys = 128;
			})
				.AddDefaultTokenProviders();

			services.AddIdentityServer()
				.AddApiAuthorization<ApplicationUser, ApplicationDbContext>()
				.AddProfileService<CustomProfileService>();

			services.AddAuthentication()
				.AddJwtBearer();

			services.AddTransient<IClaimsTransformation, CustomClaimsTransform>();

			return services.ConfigureApplicationCookie(options =>
			{
				// Return 401 when user is not authorized
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
		}

		public static IServiceCollection AddCustomizedAuthorization(this IServiceCollection services, params string[] authenticationSchemes)
		{
			return services.AddAuthorization(options =>
			{
				options.AddPolicy(Policies.Default, policy =>
				{
					policy.AddAuthenticationSchemes(authenticationSchemes);
					policy.RequireAuthenticatedUser();
				});
				options.AddPolicy(Policies.IsStudent, policy =>
				{
					policy.AddAuthenticationSchemes(authenticationSchemes);
					policy.RequireAuthenticatedUser();
					policy.RequireClaim(JwtClaimTypes.Role, Roles.Student);
				});
				options.AddPolicy(Policies.IsTeacher, policy =>
				{
					policy.AddAuthenticationSchemes(authenticationSchemes);
					policy.RequireAuthenticatedUser();
					policy.RequireClaim(JwtClaimTypes.Role, Roles.Teacher);
				});
				options.AddPolicy(Policies.HasAdministratorPermission, policy =>
				{
					policy.AddAuthenticationSchemes(authenticationSchemes);
					policy.RequireAuthenticatedUser();
					policy.RequireClaim(JwtClaimTypes.Role, Roles.Administrator);
				});
				options.DefaultPolicy = options.GetPolicy(Policies.Default)!;
			});
		}

		public static IServiceCollection AddFluentValidation(this IServiceCollection services)
		{
			services.AddValidatorsFromAssemblyContaining<LogInDtoValidator>();
			return services.AddFluentValidationAutoValidation(configuration =>
			{
				// Don't disable built in validation because without it some auth logic
				// doesn't work
				configuration.DisableBuiltInModelValidation = false;

				// Only validate controllers decorated with the `FluentValidationAutoValidation` attribute.
				configuration.ValidationStrategy = ValidationStrategy.Annotations;

				// Enable validation for parameters bound from the `BindingSource.Body` binding source.
				configuration.EnableBodyBindingSourceAutomaticValidation = true;

				// Enable validation for parameters bound from the `BindingSource.Query` binding source.
				configuration.EnableQueryBindingSourceAutomaticValidation = true;
			});
		}

		public static IServiceCollection ConfigureAppServices(this IServiceCollection services)
		{
			return services
				.AddScoped<IAuthService, AuthService>()
				.AddScoped<IAuthHelper, AuthHelper>();
		}

		public static IMvcBuilder ConfigureControllers(this IServiceCollection builder)
		{
			return builder
				.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
				})
				.ConfigureApiBehaviorOptions(x => { x.SuppressMapClientErrors = true; });
		}
	}
}
