using EUniversity.Core.Dtos.University;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers.University
{
    public class ClassroomsControllerTests : ControllersTest
    {
        public static readonly CreateClassromDto ValidCreateClassroomDto = new("300");

        public const string GetByIdRoute = "/api/classrooms/1";
        public const string PostRoute = "/api/classrooms";
        public const string PutRoute = "/api/classrooms/1";
        public const string DeleteRoute = "/api/classrooms/1";

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
