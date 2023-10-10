using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers
{
    public class UsersControllerTests : ControllersTest
    {
        public readonly RegisterDto RegisterUser1 = new("example-email1@email.com", "Test1", "Test1");
        public readonly RegisterDto RegisterUser2 = new("example-email2@email.com", "Test2", "Test2");
        public const int SampleRegisterUsersCount = 2;
        public RegisterUsersDto SampleRegisterUsers => new(
            new RegisterDto[SampleRegisterUsersCount]
            {
                RegisterUser1,
                RegisterUser2
            });

        public static readonly string[] GetMethods =
        {
            "/api/users",
            "/api/users/students",
            "/api/users/teachers"
        };
        public static readonly string[] RolesGetMethods =
        {
            "/api/users/students",
            "/api/users/teachers"
        };
        public static readonly string[] RegisterMethods =
        {
            "/api/users/students",
            "/api/users/teachers"
        };
        public static readonly (string, string)[] RegisterMethodsWithRoles =
        {
            (RolesGetMethods[0], Roles.Student),
            (RolesGetMethods[1], Roles.Teacher)
        };

        [Test]
        [TestCaseSource(nameof(GetMethods))]
        public async Task GetMethods_AdministratorRole_SucceedAndReturnValidType(string method)
        {
            // Arrange
            using var client = CreateAdministratorClient();

            // Act
            var result = await client.GetAsync(method);

            // Assert
            result.EnsureSuccessStatusCode();
            var users = await result.Content.ReadFromJsonAsync<Page<UserViewDto>>();
            Assert.That(users, Is.Not.Null);
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
        public async Task RegisterMethods_MalformedInput_Return400BadRequest(string method)
        {
            // Arrange
            using var client = CreateAdministratorClient();
            RegisterUsersDto users = new(Enumerable.Empty<RegisterDto>());

            // Act
            var result = await client.PostAsJsonAsync(method, users);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
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
        [TestCaseSource(nameof(RegisterMethodsWithRoles))]
        public async Task RegisterMethods_ValidInput_RegisterValidRoles(
            (string, string) method)
        {
            // Arrange
            using var client = CreateAdministratorClient();
            WebApplicationFactory.AuthServiceMock
                .RegisterManyAsync(Arg.Any<IEnumerable<RegisterDto>>(), Arg.Any<string>())
                .Returns(AsyncEnumerable.Repeat<RegisterResult>(
                    new(IdentityResult.Success), SampleRegisterUsersCount));

            // Act
            var result = await client.PostAsJsonAsync(method.Item1, SampleRegisterUsers);

            // Assert
            result.EnsureSuccessStatusCode();
            WebApplicationFactory.AuthServiceMock
                .Received(1)
                .RegisterManyAsync(Arg.Any<IEnumerable<RegisterDto>>(), method.Item2);
        }

        [Test]
        [TestCaseSource(nameof(RegisterMethods))]
        public async Task RegisterMethods_ValidInput_ReturnValidType(string method)
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
            var users = await result.Content.ReadFromJsonAsync<IEnumerable<CreatedUserDto>>();
            Assert.That(users, Is.Not.Null);
        }
    }
}
