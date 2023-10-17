using EUniversity.Core.Models;
using EUniversity.Core.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers
{
    /// <summary>
    /// Class that implements base CRUD controller tests.
    /// </summary>
    public abstract class CrudControllersTest<TEntity, TId, TDetailsDto, TCreateDto, TUpdateDto>
        : ControllersTest
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
        where TDetailsDto : class, IEquatable<TDetailsDto>
        where TCreateDto : class, IEquatable<TCreateDto>
        where TUpdateDto : class, IEquatable<TUpdateDto>
    {
        /// <summary>
        /// Mock of the CRUD service. Should be initialized in <see cref="SetUpService"/>
        /// </summary>
        protected ICrudService<TEntity, TId, TDetailsDto, TCreateDto, TUpdateDto> ServiceMock
        { get; set; } = null!;

        /// <summary>
        /// When implemented, gets test <typeparamref name="TDetailsDto"/>.
        /// </summary>
        /// <returns>
        /// Test <typeparamref name="TDetailsDto"/>.
        /// </returns>
        public abstract TDetailsDto GetTestDetailsDto();

        /// <summary>
        /// When implemented, gets valid <typeparamref name="TCreateDto"/>.
        /// </summary>
        /// <returns>
        /// Valid <typeparamref name="TCreateDto"/>.
        /// </returns>
        protected abstract TCreateDto GetValidCreateDto();

        /// <summary>
        /// When implemented, gets invalid <typeparamref name="TCreateDto"/>.
        /// </summary>
        /// <returns>
        /// Invalid <typeparamref name="TCreateDto"/>.
        /// </returns>
        protected abstract TCreateDto GetInvalidCreateDto();

        /// <summary>
        /// When implemented, gets valid <typeparamref name="TUpdateDto"/>.
        /// </summary>
        /// <returns>
        /// Valid <typeparamref name="TUpdateDto"/>.
        /// </returns>
        protected abstract TUpdateDto GetValidUpdateDto();

        /// <summary>
        /// When implemented, gets invalid <typeparamref name="TUpdateDto"/>.
        /// </summary>
        /// <returns>
        /// Invalid <typeparamref name="TUpdateDto"/>.
        /// </returns>
        protected abstract TUpdateDto GetInvalidUpdateDto();

        /// <summary>
        /// Gets HTTP client used for testing [Authorize] methods. 
        /// Uses client with 'Administrator' role by default.
        /// </summary>
        /// <returns>
        /// HTTP client for testing [Authorize] methods.
        /// </returns>
        protected virtual HttpClient GetTestClient() => CreateAdministratorClient();

        [SetUp]
        public abstract void SetUpService();

        /// <summary>
        /// Route of HTTP GET for getting an element by its ID.
        /// </summary>
        /// <remarks>
        /// Authentication is not checked.
        /// </remarks>
        public abstract string GetByIdRoute { get; }
        /// <summary>
        /// Route of HTTP POST.
        /// </summary>
        /// <remarks>
        /// Authentication is checked.
        /// </remarks>
        public abstract string PostRoute { get; }
        /// <summary>
        /// Route of HTTP PUT.
        /// </summary>
        /// <remarks>
        /// Authentication is checked.
        /// </remarks>
        public abstract string PutRoute { get; }
        /// <summary>
        /// Route of HTTP DELETE.
        /// </summary>
        /// <remarks>
        /// Authentication is checked.
        /// </remarks>
        public abstract string DeleteRoute { get; }

        /// <summary>
        /// Default ID to be used for mocking.
        /// </summary>
        public abstract TId DefaultId { get; }

        [Test]
        public virtual async Task GetById_ValidCall_SucceedsAndReturnsValidType()
        {
            // Arrange
            TDetailsDto expectedDto = GetTestDetailsDto();
            ServiceMock
                .GetByIdAsync(Arg.Any<TId>())
                .Returns(expectedDto);
            using var client = GetTestClient();

            // Act
            var result = await client.GetAsync(GetByIdRoute);

            // Assert
            await ServiceMock
                .Received(1)
                .GetByIdAsync(DefaultId);
            result.EnsureSuccessStatusCode();
            var dto = await result.Content.ReadFromJsonAsync<TDetailsDto>();
            Assert.That(dto, Is.EqualTo(expectedDto));
        }

        [Test]
        public virtual async Task GetById_ElementDoesNotExist_Return404NotFound()
        {
            // Arrange
            ServiceMock
                .GetByIdAsync(Arg.Any<TId>())
                .ReturnsNull();
            using var client = GetTestClient();

            // Act
            var result = await client.GetAsync(GetByIdRoute);

            // Assert
            await ServiceMock
                .Received(1)
                .GetByIdAsync(DefaultId);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public virtual async Task Post_ValidCall_Succeeds()
        {
            // Arrange
            TCreateDto validCreateDto = GetValidCreateDto();
            using var client = GetTestClient();
            ServiceMock
                .CreateAsync(Arg.Any<TCreateDto>())
                .Returns(DefaultId);

            // Act
            var result = await client.PostAsJsonAsync(PostRoute, validCreateDto);

            // Assert
            result.EnsureSuccessStatusCode();
            await ServiceMock
                .Received(1)
                .CreateAsync(validCreateDto);
        }

        [Test]
        public virtual async Task Post_InvalidInput_Returns400BadRequest()
        {
            // Arrange
            using var client = GetTestClient();
            ServiceMock
                .CreateAsync(Arg.Any<TCreateDto>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.PostAsJsonAsync(PostRoute, GetInvalidCreateDto());

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public virtual async Task Post_UnauthenticatedUser_Returns401Unauthorized()
        {
            // Arrange
            using var client = CreateUnauthorizedClient();
            ServiceMock
                .CreateAsync(Arg.Any<TCreateDto>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.PostAsJsonAsync(PostRoute, GetValidCreateDto());

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public virtual async Task Put_ValidCall_Succeeds()
        {
            // Arrange
            TUpdateDto validUpdateDto = GetValidUpdateDto();
            using var client = GetTestClient();
            ServiceMock
                .UpdateAsync(Arg.Any<TId>(), Arg.Any<TUpdateDto>())
                .Returns(true);

            // Act
            var result = await client.PutAsJsonAsync(PutRoute, validUpdateDto);

            // Assert
            await ServiceMock
                .Received(1)
                .UpdateAsync(DefaultId, validUpdateDto);
            result.EnsureSuccessStatusCode();
        }

        [Test]
        public virtual async Task Put_ElementDoesNotExist_Returns404NotFound()
        {
            // Arrange
            TUpdateDto validUpdateDto = GetValidUpdateDto();
            using var client = GetTestClient();
            ServiceMock
                .UpdateAsync(Arg.Any<TId>(), Arg.Any<TUpdateDto>())
                .Returns(false);

            // Act
            var result = await client.PutAsJsonAsync(PutRoute, validUpdateDto);

            // Assert
            await ServiceMock
                .Received(1)
                .UpdateAsync(DefaultId, validUpdateDto);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public virtual async Task Put_InvalidInput_Returns400BadRequest()
        {
            // Arrange
            using var client = GetTestClient();
            ServiceMock
                .UpdateAsync(Arg.Any<TId>(), Arg.Any<TUpdateDto>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.PutAsJsonAsync(PutRoute, GetInvalidUpdateDto());

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public virtual async Task Put_UnauthenticatedUser_Returns401Unauthorized()
        {
            // Arrange
            using var client = CreateUnauthorizedClient();
            ServiceMock
                .UpdateAsync(Arg.Any<TId>(), Arg.Any<TUpdateDto>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.PutAsJsonAsync(PutRoute, GetValidUpdateDto());

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public virtual async Task Delete_ValidCall_Succeeds()
        {
            // Arrange
            using var client = GetTestClient();
            ServiceMock
                .DeleteAsync(Arg.Any<TId>())
                .Returns(true);

            // Act
            var result = await client.DeleteAsync(DeleteRoute);

            // Assert
            await ServiceMock
                .Received(1)
                .DeleteAsync(DefaultId);
            result.EnsureSuccessStatusCode();
        }

        [Test]
        public virtual async Task Delete_ElementDoesNotExist_Return404NotFound()
        {
            // Arrange
            using var client = GetTestClient();
            ServiceMock
                .DeleteAsync(Arg.Any<TId>())
                .Returns(false);

            // Act
            var result = await client.DeleteAsync(DeleteRoute);

            // Assert
            await ServiceMock
                .Received(1)
                .DeleteAsync(DefaultId);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Delete_UnauthenticatedUser_Returns401Unauthorized()
        {
            // Arrange
            using var client = CreateUnauthorizedClient();
            ServiceMock
                .DeleteAsync(Arg.Any<TId>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.DeleteAsync(DeleteRoute);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
