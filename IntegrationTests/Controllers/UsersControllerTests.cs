using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Models;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using Mapster;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers
{
	public class UsersControllerTests : ControllersTest
	{
		public readonly RegisterDto RegisterUser1 = new()
		{
			Email = "example-email1@email.com",
			FirstName = "Test1",
			LastName = "Test1"
		};
		public readonly RegisterDto RegisterUser2 = new()
		{
			Email = "example-email2@email.com",
			FirstName = "Test2",
			LastName = "Test2"
		};
		public const int SampleRegisterUsersCount = 2;
		public RegisterUsersDto SampleRegisterUsers => new()
		{
			Users = new RegisterDto[SampleRegisterUsersCount]
			{
				RegisterUser1,
				RegisterUser2
			}
		};

		public static readonly string[] GetMethods =
		{
			"/api/users",
			"/api/users/students",
			"/api/users/teachers"
		};
		public static readonly string[] RegisterMethods =
		{
			"/api/users/students",
			"/api/users/teachers"
		};
		public static readonly string[][] RegisterMethodsRoles =
		{
			new [] { Roles.Student },
			new [] { Roles.Teacher }
		};

		[Test]
		[TestCaseSource(nameof(GetMethods))]
		public async Task GetMethods_AdministratorRole_SucceedAndReturnValidType(string method)
		{
			// Arrange
			using var client = CreateAdministratorClient();
			List<ApplicationUser> output = new()
			{
				new ApplicationUser() {Id = "1", FirstName = "Joe", LastName = "Doe", Email = "joe@doe.com", UserName = "jd"},
				new ApplicationUser() {Id = "2", FirstName = "Jane", LastName = "Doe", Email = "jane@doe.com", UserName = "jnd"},
			};
			WebApplicationFactory.UserManagerMock
				.GetUsersInRoleAsync(Arg.Any<string>())
				.Returns(output);

			// Act
			var result = await client.GetAsync(method);

			// Assert
			result.EnsureSuccessStatusCode();
			var users = await result.Content.ReadFromJsonAsync<UsersViewDto>();
			Assert.That(users, Is.Not.Null);
			CollectionAssert.AreEqual(output.Adapt<List<UserViewDto>>(), users.Users);
		}

		[Test]
		[TestCaseSource(nameof(GetMethods))]
		public async Task GetMethods_UnauthenticatedUser_Return401Unauthorized(string method)
		{
			// Arrange
			using var client = CreateUnauthorizedClient();

			// Act
			var result = await client.GetAsync(method);

			// Assert
			Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
		}

		[Test]
		[TestCaseSource(nameof(GetMethods))]
		public async Task GetMethods_NoAdministratorRole_Return403Forbidden(string method)
		{
			// Arrange
			using var client = CreateStudentClient();

			// Act
			var result = await client.GetAsync(method);

			// Assert
			Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
		}

		[Test]
		[TestCaseSource(nameof(RegisterMethods))]
		public async Task RegisterMethods_ValidInput_Succeed(string method)
		{
			// Arrange
			using var client = CreateAdministratorClient();
			WebApplicationFactory.AuthServiceMock
				.RegisterAsync(Arg.Any<RegisterDto>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string[]>())
				.Returns(Task.FromResult(new RegisterResult(IdentityResult.Success)));

			// Act
			var result = await client.PostAsJsonAsync(method, SampleRegisterUsers);

			// Assert
			result.EnsureSuccessStatusCode();
			await WebApplicationFactory.AuthServiceMock
				.Received(SampleRegisterUsersCount)
				.RegisterAsync(Arg.Any<RegisterDto>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string[]>());
		}

		[Test]
		[TestCaseSource(nameof(RegisterMethods))]
		public async Task RegisterMethods_PartiallyValidInput_Succeed(string method)
		{
			// Arrange
			using var client = CreateAdministratorClient();
			WebApplicationFactory.AuthServiceMock
				.RegisterAsync(RegisterUser1, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string[]>())
				.Returns(Task.FromResult(new RegisterResult(IdentityResult.Failed())));
			WebApplicationFactory.AuthServiceMock
				.RegisterAsync(RegisterUser2, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string[]>())
				.Returns(Task.FromResult(new RegisterResult(IdentityResult.Success)));

			// Act
			var result = await client.PostAsJsonAsync(method, SampleRegisterUsers);

			// Assert
			result.EnsureSuccessStatusCode();
			await WebApplicationFactory.AuthServiceMock
				.Received(SampleRegisterUsersCount)
				.RegisterAsync(Arg.Any<RegisterDto>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string[]>());
		}

		[Test]
		[TestCaseSource(nameof(RegisterMethods))]
		public async Task RegisterMethods_MalformedInput_Return400BadRequest(string method)
		{
			// Arrange
			using var client = CreateAdministratorClient();
			RegisterUsersDto users = new()
			{
				Users = Enumerable.Empty<RegisterDto>()
			};

			// Act
			var result = await client.PostAsJsonAsync(method, users);

			// Assert
			Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}

		[Test]
		[TestCaseSource(nameof(RegisterMethods))]
		public async Task RegisterMethods_InvalidInput_Return400BadRequest(string method)
		{
			// Arrange
			using var client = CreateAdministratorClient();
			WebApplicationFactory.AuthServiceMock
				.RegisterAsync(Arg.Any<RegisterDto>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string[]>())
				.Returns(Task.FromResult(new RegisterResult(IdentityResult.Failed())));

			// Act
			var result = await client.PostAsJsonAsync(method, SampleRegisterUsers);

			// Assert
			Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
			await WebApplicationFactory.AuthServiceMock
				.Received(SampleRegisterUsersCount)
				.RegisterAsync(Arg.Any<RegisterDto>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string[]>());
		}

		[Test]
		[TestCaseSource(nameof(RegisterMethods))]
		public async Task RegisterMethods_UnauthenticatedUser_Return401Unauthorized(string method)
		{
			// Arrange
			using var client = CreateUnauthorizedClient();

			// Act
			var result = await client.PostAsJsonAsync(method, SampleRegisterUsers);

			// Assert
			Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
		}

		[Test]
		[TestCaseSource(nameof(RegisterMethods))]
		public async Task RegisterMethods_NoAdministratorRole_Return403Forbidden(string method)
		{
			// Arrange
			using var client = CreateStudentClient();

			// Act
			var result = await client.PostAsJsonAsync(method, SampleRegisterUsers);

			// Assert
			Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
		}

		[Test]
		public async Task RegisterMethods_ValidInput_RegisterValidRoles(
			[ValueSource(nameof(RegisterMethods))] string method, 
			[ValueSource(nameof(RegisterMethodsRoles))] string[] roles)
		{
			// Arrange
			using var client = CreateAdministratorClient();
			WebApplicationFactory.AuthServiceMock
				.RegisterAsync(Arg.Any<RegisterDto>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string[]>())
				.Returns(Task.FromResult(new RegisterResult(IdentityResult.Success)));

			// Act
			var result = await client.PostAsJsonAsync(method, SampleRegisterUsers);

			// Assert
			result.EnsureSuccessStatusCode();
			await WebApplicationFactory.AuthServiceMock
				.Received(SampleRegisterUsersCount)
				.RegisterAsync(Arg.Any<RegisterDto>(), Arg.Any<string>(), Arg.Any<string>(), roles);
		}
	}
}
