using EUniversity.Core.Models;
using EUniversity.Core.Policy;
using System.Reflection;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Extensions
{
	public static class WebApplicationExtensions
	{
		public static WebApplication CreateRoles(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();

			var roleManager =
				scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			// Select all constants from Roles class
			var roles = typeof(Roles)
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Where(f => f.IsLiteral && !f.IsInitOnly)
				.Select(f => f.GetRawConstantValue()!.ToString()!);

			foreach (var role in roles)
			{
				if (!roleManager.RoleExistsAsync(role).Result)
				{
					roleManager.CreateAsync(new(role)).Wait();
				}
			}
			return app;
		}

		public static WebApplication CreateAdministrator(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();

			var userManager =
				scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			string userName = "admin";
			string email = "admin@e-university.com";
			string password = "Chang3M3InProduct10nPlz!";

			if (userManager.FindByEmailAsync(email).Result == null)
			{
				ApplicationUser user = new()
				{
					UserName = userName,
					Email = email,
					FirstName = "Admino",
					LastName = "Guru"
				};

				userManager.CreateAsync(user, password).Wait();

				userManager.AddToRoleAsync(user, Roles.Administrator).Wait();
			}
			return app;
		}
	}
}
