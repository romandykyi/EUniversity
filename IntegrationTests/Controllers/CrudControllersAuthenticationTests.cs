using EUniversity.IntegrationTests.Controllers.University;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers
{
    public class CrudControllersAuthenticationTests : ControllersTest
    {
        private const int DefaultIntId = 1;
        private static string IntIdRoute(string route) => string.Format(route, DefaultIntId);

        public static readonly IEnumerable<string> AdminGetMethods = Enumerable.Empty<string>();
        public static readonly IEnumerable<string> AuthorizeGetMethods = Enumerable.Empty<string>();

        public static readonly IEnumerable<(string, object?)> AdminPostMethods = new[]
        {
            (ClassroomsControllerTests.CreateRoute, (object?)ClassroomsControllerTests.ValidCreateClassroomDto)
        };
        public static readonly IEnumerable<(string, object?)> AuthorizePostMethods = AdminPostMethods;

        public static readonly IEnumerable<(string, object?)> AdminPutMethods = new[]
        {
            (IntIdRoute(ClassroomsControllerTests.UpdateRoute), (object?)ClassroomsControllerTests.ValidCreateClassroomDto)
        };
        public static readonly IEnumerable<(string, object?)> AuthorizePutMethods =
            AdminPutMethods;

        public static readonly IEnumerable<string> AdminDeleteMethods = new[]
        {
            IntIdRoute(ClassroomsControllerTests.DeleteRoute)
        };
        public static readonly IEnumerable<string> AuthorizeDeleteMethods =
            AdminDeleteMethods;

        [Test]
        [TestCaseSource(nameof(AuthorizeGetMethods))]
        public async Task AuthorizeGetMethods_UnauthenticatedUser_Returns401Unauthorized(string method)
        {
            // Arrange
            using var client = CreateUnauthorizedClient();

            // Act
            var result = await client.GetAsync(method);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        [TestCaseSource(nameof(AdminGetMethods))]
        public async Task AdminGetMethods_StudentRole_Return403Forbidden(string method)
        {
            // Arrange
            using var client = CreateStudentClient();

            // Act
            var result = await client.GetAsync(method);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        [TestCaseSource(nameof(AuthorizePostMethods))]
        public async Task AuthorizePostMethods_UnauthenticatedUser_Returns401Unauthorized((string, object?) method)
        {
            // Arrange
            using var client = CreateUnauthorizedClient();

            // Act
            var result = await client.PostAsJsonAsync(method.Item1, method.Item2);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        [TestCaseSource(nameof(AdminPostMethods))]
        public async Task AdminPostMethods_StudentRole_Return403Forbidden((string, object?) method)
        {
            // Arrange
            using var client = CreateStudentClient();

            // Act
            var result = await client.PostAsJsonAsync(method.Item1, method.Item2);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        [TestCaseSource(nameof(AuthorizePutMethods))]
        public async Task AuthorizePutMethods_UnauthenticatedUser_Returns401Unauthorized((string, object?) method)
        {
            // Arrange
            using var client = CreateUnauthorizedClient();

            // Act
            var result = await client.PutAsJsonAsync(method.Item1, method.Item2);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        [TestCaseSource(nameof(AdminPutMethods))]
        public async Task AdminPutMethods_StudentRole_Return403Forbidden((string, object?) method)
        {
            // Arrange
            using var client = CreateStudentClient();

            // Act
            var result = await client.PutAsJsonAsync(method.Item1, method.Item2);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        [TestCaseSource(nameof(AuthorizeDeleteMethods))]
        public async Task AuthorizeDeleteMethods_UnauthenticatedUser_Returns401Unauthorized(string method)
        {
            // Arrange
            using var client = CreateUnauthorizedClient();

            // Act
            var result = await client.DeleteAsync(method);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        [TestCaseSource(nameof(AdminDeleteMethods))]
        public async Task AdminDeleteMethods_StudentRole_Return403Forbidden(string method)
        {
            // Arrange
            using var client = CreateStudentClient();

            // Act
            var result = await client.DeleteAsync(method);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
