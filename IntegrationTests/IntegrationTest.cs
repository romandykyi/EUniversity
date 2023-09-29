using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using EUniversity.Infrastructure.Data;
using EUniversity.IntegrationTests.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace EUniversity.IntegrationTests
{
	/// <summary>
	/// Base class for integration tests.
	/// </summary>
	/// <remarks>
	/// Each test in derived classes is isolated, and no changes to the database will be saved.
	/// </remarks>
	public abstract class IntegrationTest
	{
		protected ProgramWebApplicationFactory WebApplicationFactory { get; private set; }
		protected HttpClient Client { get; private set; }
		protected IServiceScopeFactory ScopeFactory { get; private set; }
		protected IServiceScope ServiceScope { get; private set; }
		protected ApplicationDbContext DbContext { get; private set; }
		protected UserManager<ApplicationUser> UserManager { get; private set; }
		protected IAuthService AuthService { get; private set; }

		/// <summary>
		/// Password of all default users.
		/// </summary>
		public const string DefaultUsersPassword = "Password123!";

		public const string DefaultAdminUserName = "administrator";
		public const string DefaultStudentUserName = "student";
		public const string DefaultTeacherUserName = "teacher";

		[SetUp]
		public void SetUpIntegration()
		{
			WebApplicationFactory = new ProgramWebApplicationFactory();
			WebApplicationFactory.Server.PreserveExecutionContext = true;
			Client = WebApplicationFactory.CreateClient();

			ScopeFactory = WebApplicationFactory.Services.GetService<IServiceScopeFactory>()!;
			ServiceScope = ScopeFactory.CreateScope();
			DbContext = ServiceScope.ServiceProvider.GetService<ApplicationDbContext>()!;
			UserManager = ServiceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>()!;
			AuthService = ServiceScope.ServiceProvider.GetService<IAuthService>()!;

			// Begin transaction to make tests isolated
			DbContext.Database.BeginTransaction(IsolationLevel.Serializable);
		}

		[TearDown]
		public void TearDownIntegration()
		{
			// Undo all changes
			DbContext.ChangeTracker.Clear();

			ServiceScope.Dispose();
			Client.Dispose();
			WebApplicationFactory.Dispose();
		}

		/// <summary>
		/// Ensures that username is free.
		/// </summary>
		/// <param name="userName">Email that needs to be free.</param>
		protected async Task ClearUserNameAsync(string userName)
		{
			var user = await UserManager.FindByNameAsync(userName);
			if (user != null)
			{
				await UserManager.DeleteAsync(user);
			}
		}

		/// <summary>
		/// Ensures that email is free.
		/// </summary>
		/// <param name="userName">Email that needs to be free.</param>
		protected async Task ClearEmailAsync(string email)
		{
			var user = await UserManager.FindByEmailAsync(email);
			if (user != null)
			{
				await UserManager.DeleteAsync(user);
			}
		}

		/// <summary>
		/// Registers user with default password.
		/// </summary>
		protected async Task RegisterDefaultUserAsync(string userName, string email, params string[] roles)
		{
			await ClearUserNameAsync(userName);
			await ClearEmailAsync(email);

			var registerData = new RegisterDto()
			{
				Email = email,
				FirstName = userName,
				LastName = userName
			};
			var result = await AuthService.RegisterAsync(registerData, userName, DefaultUsersPassword, roles);

			// Make sure that user is registered
			Assert.That(result.Succeeded,
				$"Failed to register user \"{userName}\" with email \"{email}\"");
		}

		/// <summary>
		/// Registers student with default password.
		/// </summary>
		protected async Task RegisterDefaultStudentAsync()
		{
			await RegisterDefaultUserAsync(DefaultStudentUserName,
				"student@e-university.com", Roles.Student);
		}

		/// <summary>
		/// Registers teacher with default password.
		/// </summary>
		protected async Task RegisterDefaultTeacherAsync()
		{
			await RegisterDefaultUserAsync(DefaultTeacherUserName,
				"teacher@e-university.com", Roles.Teacher);
		}

		/// <summary>
		/// Registers administrator with default password.
		/// </summary>
		protected async Task RegisterDefaultAdminAsync()
		{
			await RegisterDefaultUserAsync(DefaultAdminUserName,
				"admin@e-university.com", Roles.Administrator);
		}

		/// <summary>
		/// Logs in.
		/// </summary>
		protected async Task LogIn(string userName, string password)
		{
			var result = await AuthService.LogInAsync(
				new() { UserName = userName, Password = password }
				);

			Assert.That(result, "Failed to log in");
		}
	}
}
