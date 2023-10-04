using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EUniversity.IntegrationTests.Services
{
    public class AuthServiceTests : ServicesTest
    {
        private const string DefaultUserName = "user";
        private const string DefaultPassword = "Password1!@Gs";
        private const string NewPassword = DefaultPassword + "2";
        private const string DefaultEmail = "test-email@e-university.com";

        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private IAuthService _authService;

        // Ensures that username is free.
        private async Task ClearUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        // Ensures that email is free.
        private async Task ClearEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        private async Task<RegisterDto> GetDefaultRegisterDtoAsync()
        {
            RegisterDto result = new("example@email.com", "Joe", "Doe");
            await ClearEmailAsync(result.Email);
            return result;
        }

        // Registers default user and returns it
        private async Task<ApplicationUser> RegisterDefaultUserAsync()
        {
            ApplicationUser user = new()
            {
                UserName = DefaultUserName,
                FirstName = "First",
                LastName = "Last",
                Email = DefaultEmail
            };
            await ClearEmailAsync(user.Email);
            await ClearUserNameAsync(user.UserName);

            var result = await _userManager.CreateAsync(user, DefaultPassword);
            Assert.That(result.Succeeded, "Failed to register default user");

            return user;
        }

        [SetUp]
        public void SetUp()
        {
            _signInManager = ServiceScope.ServiceProvider.GetService<SignInManager<ApplicationUser>>()!;
            _userManager = ServiceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>()!;
            _authService = ServiceScope.ServiceProvider.GetService<IAuthService>()!;
        }

        [Test]
        public async Task Register_CustomUserNameAndPassword_Succeeds()
        {
            // Arrange
            await ClearUserNameAsync(DefaultUserName);
            var registerDto = await GetDefaultRegisterDtoAsync();

            // Act
            var result = await _authService.RegisterAsync(registerDto, DefaultUserName, DefaultPassword);

            // Assert
            Assert.Multiple(async () =>
            {
                Assert.That(result.Succeeded);
                Assert.That(await _userManager.FindByNameAsync(DefaultUserName), Is.Not.Null);
            });
        }

        [Test]
        public async Task Register_NoUserName_ReturnsValidUsername()
        {
            // Arrange
            var registerDto = await GetDefaultRegisterDtoAsync();

            // Act
            var result = await _authService.RegisterAsync(registerDto, password: DefaultPassword);

            // Assert
            Assert.Multiple(async () =>
            {
                Assert.That(result.Succeeded);
                Assert.That(await _userManager.FindByNameAsync(result.UserName), Is.Not.Null);
            });
        }

        [Test]
        public async Task Register_NoPassword_ReturnsValidPassword()
        {
            // Arrange
            await ClearUserNameAsync(DefaultUserName);
            var registerDto = await GetDefaultRegisterDtoAsync();

            // Act
            var result = await _authService.RegisterAsync(registerDto, userName: DefaultUserName);

            // Assert
            Assert.That(result.Succeeded);
            var user = await _userManager.FindByNameAsync(DefaultUserName);
            Assert.That(user, Is.Not.Null);
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, result.Password, false);
            Assert.That(signInResult.Succeeded);
        }

        [Test]
        public async Task Register_WithRoles_Succeeds()
        {
            // Arrange
            string[] roles = { Roles.Administrator, Roles.Teacher };
            await ClearUserNameAsync(DefaultUserName);
            var registerDto = await GetDefaultRegisterDtoAsync();

            // Act
            var result = await _authService.RegisterAsync(
                registerDto, DefaultUserName, DefaultPassword, roles);

            // Assert
            Assert.That(result.Succeeded);
            var user = await _userManager.FindByNameAsync(DefaultUserName);
            Assert.That(user, Is.Not.Null);
            foreach (var role in roles)
            {
                Assert.That(await _userManager.IsInRoleAsync(user, role));
            }
        }

        // Integration tests for RegisterManyAsync are redundant,
        // because this method just calls RegisterAsync

        // Testing LogIn is not possible here, because it requires HttpContext

        [Test]
        public async Task ChangePassword_ValidPassword_Succeeds()
        {
            // Arrange
            var user = await RegisterDefaultUserAsync();
            ChangePasswordDto password = new(DefaultPassword, NewPassword);

            // Act
            var result = await _authService.ChangePasswordAsync(user.Id, password);

            // Assert
            Assert.That(result.Succeeded);
            var passwordCheckResult =
                await _signInManager.CheckPasswordSignInAsync(user, password.New, false);
            Assert.That(passwordCheckResult.Succeeded);
        }

        [Test]
        public async Task ChangePassword_InvalidPassword_Fails()
        {
            // Arrange
            var user = await RegisterDefaultUserAsync();
            ChangePasswordDto password = new("invalid", NewPassword);

            // Act
            var result = await _authService.ChangePasswordAsync(user.Id, password);

            // Assert
            Assert.That(result.Succeeded, Is.False);
        }
    }
}
