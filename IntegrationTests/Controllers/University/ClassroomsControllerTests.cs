using EUniversity.Core.Dtos.University;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers.University
{
    public class ClassroomsControllerTests : ControllersTest
    {
        public static readonly CreateClassromDto InvalidCreateClassroomDto = new(string.Empty);
        public static readonly CreateClassromDto ValidCreateClassroomDto = new("300");

        public const int DefaultIntId = 1;
        public static readonly string GetByIdRoute = $"/api/classrooms/{DefaultIntId}";
        public const string PostRoute = "/api/classrooms";
        public static readonly string PutRoute = $"/api/classrooms/{DefaultIntId}";
        public static readonly string DeleteRoute = $"/api/classrooms/{DefaultIntId}";

        [Test]
        public async Task GetById_ValidCall_SucceedsAndReturnsValidType()
        {
            // Arrange
            ViewClassroomDto expectedDto = new("101");
            WebApplicationFactory.ClassroomsServiceMock
                .GetByIdAsync(Arg.Any<int>())
                .Returns(expectedDto);
            using var client = CreateAdministratorClient();

            // Act
            var result = await client.GetAsync(GetByIdRoute);

            // Assert
            await WebApplicationFactory.ClassroomsServiceMock
                .Received(1)
                .GetByIdAsync(DefaultIntId);
            result.EnsureSuccessStatusCode();
            var dto = await result.Content.ReadFromJsonAsync<ViewClassroomDto>();
            Assert.That(dto, Is.EqualTo(expectedDto));
        }

        [Test]
        public async Task GetById_ElementDoesNotExist_Return404NotFound()
        {
            // Arrange
            WebApplicationFactory.ClassroomsServiceMock
                .GetByIdAsync(Arg.Any<int>())
                .ReturnsNull();
            using var client = CreateAdministratorClient();

            // Act
            var result = await client.GetAsync(GetByIdRoute);

            // Assert
            await WebApplicationFactory.ClassroomsServiceMock
                .Received(1)
                .GetByIdAsync(DefaultIntId);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetById_UnauthenticatedUser_Returns401Unauthorized()
        {
            // Arrange
            using var client = CreateUnauthorizedClient();
            WebApplicationFactory.ClassroomsServiceMock
                .GetByIdAsync(Arg.Any<int>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.GetAsync(GetByIdRoute);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Post_ValidCall_Succeeds()
        {
            // Arrange
            using var client = CreateAdministratorClient();
            WebApplicationFactory.ClassroomsServiceMock
                .CreateAsync(Arg.Any<CreateClassromDto>())
                .Returns(2);

            // Act
            var result = await client.PostAsJsonAsync(PostRoute, ValidCreateClassroomDto);

            // Assert
            result.EnsureSuccessStatusCode();
            await WebApplicationFactory.ClassroomsServiceMock
                .Received(1)
                .CreateAsync(ValidCreateClassroomDto);
        }

        [Test]
        public async Task Post_InvalidInput_Returns400BadRequest()
        {
            // Arrange
            using var client = CreateAdministratorClient();
            WebApplicationFactory.ClassroomsServiceMock
                .CreateAsync(Arg.Any<CreateClassromDto>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.PostAsJsonAsync(PostRoute, InvalidCreateClassroomDto);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Post_UnauthenticatedUser_Returns401Unauthorized()
        {
            // Arrange
            using var client = CreateUnauthorizedClient();
            WebApplicationFactory.ClassroomsServiceMock
                .CreateAsync(Arg.Any<CreateClassromDto>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.PostAsJsonAsync(PostRoute, ValidCreateClassroomDto);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Post_StudentRole_Return403Forbidden()
        {
            // Arrange
            using var client = CreateStudentClient();
            WebApplicationFactory.ClassroomsServiceMock
                .CreateAsync(Arg.Any<CreateClassromDto>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.PostAsJsonAsync(PostRoute, ValidCreateClassroomDto);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public async Task Put_ValidCall_Succeeds()
        {
            // Arrange
            using var client = CreateAdministratorClient();
            WebApplicationFactory.ClassroomsServiceMock
                .UpdateAsync(Arg.Any<int>(), Arg.Any<CreateClassromDto>())
                .Returns(true);

            // Act
            var result = await client.PutAsJsonAsync(PutRoute, ValidCreateClassroomDto);

            // Assert
            await WebApplicationFactory.ClassroomsServiceMock
                .Received(1)
                .UpdateAsync(DefaultIntId, ValidCreateClassroomDto);
            result.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task Put_ElementDoesNotExist_Returns404NotFound()
        {
            // Arrange
            using var client = CreateAdministratorClient();
            WebApplicationFactory.ClassroomsServiceMock
                .UpdateAsync(Arg.Any<int>(), Arg.Any<CreateClassromDto>())
                .Returns(false);

            // Act
            var result = await client.PutAsJsonAsync(PutRoute, ValidCreateClassroomDto);

            // Assert
            await WebApplicationFactory.ClassroomsServiceMock
                .Received(1)
                .UpdateAsync(DefaultIntId, ValidCreateClassroomDto);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Put_InvalidInput_Returns400BadRequest()
        {
            // Arrange
            using var client = CreateAdministratorClient();
            WebApplicationFactory.ClassroomsServiceMock
                .UpdateAsync(Arg.Any<int>(), Arg.Any<CreateClassromDto>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.PutAsJsonAsync(PutRoute, InvalidCreateClassroomDto);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Put_UnauthenticatedUser_Returns401Unauthorized()
        {
            // Arrange
            using var client = CreateUnauthorizedClient();
            WebApplicationFactory.ClassroomsServiceMock
                .UpdateAsync(Arg.Any<int>(), Arg.Any<CreateClassromDto>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.PutAsJsonAsync(PutRoute, ValidCreateClassroomDto);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Put_StudentRole_Return403Forbidden()
        {
            // Arrange
            using var client = CreateStudentClient();
            WebApplicationFactory.ClassroomsServiceMock
                .UpdateAsync(Arg.Any<int>(), Arg.Any<CreateClassromDto>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.PutAsJsonAsync(PutRoute, ValidCreateClassroomDto);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public async Task Delete_ValidCall_Succeeds()
        {
            // Arrange
            using var client = CreateAdministratorClient();
            WebApplicationFactory.ClassroomsServiceMock
                .DeleteAsync(Arg.Any<int>())
                .Returns(true);

            // Act
            var result = await client.DeleteAsync(DeleteRoute);

            // Assert
            await WebApplicationFactory.ClassroomsServiceMock
                .Received(1)
                .DeleteAsync(DefaultIntId);
            result.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task Delete_ElementDoesNotExist_Return404NotFound()
        {
            // Arrange
            using var client = CreateAdministratorClient();
            WebApplicationFactory.ClassroomsServiceMock
                .DeleteAsync(Arg.Any<int>())
                .Returns(false);

            // Act
            var result = await client.DeleteAsync(DeleteRoute);

            // Assert
            await WebApplicationFactory.ClassroomsServiceMock
                .Received(1)
                .DeleteAsync(DefaultIntId);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Delete_UnauthenticatedUser_Returns401Unauthorized()
        {
            // Arrange
            using var client = CreateUnauthorizedClient();
            WebApplicationFactory.ClassroomsServiceMock
                .DeleteAsync(Arg.Any<int>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.DeleteAsync(DeleteRoute);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task Delete_StudentRole_Return403Forbidden()
        {
            // Arrange
            using var client = CreateStudentClient();
            WebApplicationFactory.ClassroomsServiceMock
                .DeleteAsync(Arg.Any<int>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.DeleteAsync(DeleteRoute);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
