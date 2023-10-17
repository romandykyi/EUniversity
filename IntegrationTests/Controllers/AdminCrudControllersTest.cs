using EUniversity.Core.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers
{
    /// <summary>
    /// Class that implements base CRUD controller tests, where PUT, POST and DELETE methods require 
    /// Administrator role.
    /// </summary>
    public abstract class AdminCrudControllersTest<TEntity, TId, TDetailsDto, TCreateDto, TUpdateDto> :
        CrudControllersTest<TEntity, TId, TDetailsDto, TCreateDto, TUpdateDto>
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
        where TDetailsDto : class, IEquatable<TDetailsDto>
        where TCreateDto : class, IEquatable<TCreateDto>
        where TUpdateDto : class, IEquatable<TUpdateDto>
    {
        [Test]
        public virtual async Task Post_StudentRole_Return403Forbidden()
        {
            // Arrange
            using var client = CreateStudentClient();
            ServiceMock
                .CreateAsync(Arg.Any<TCreateDto>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.PostAsJsonAsync(PostRoute, GetValidCreateDto());

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public virtual async Task Put_StudentRole_Return403Forbidden()
        {
            // Arrange
            using var client = CreateStudentClient();
            ServiceMock
                .UpdateAsync(Arg.Any<TId>(), Arg.Any<TUpdateDto>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.PutAsJsonAsync(PutRoute, GetValidUpdateDto());

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public async Task Delete_StudentRole_Return403Forbidden()
        {
            // Arrange
            using var client = CreateStudentClient();
            ServiceMock
                .DeleteAsync(Arg.Any<TId>())
                .Throws<InvalidOperationException>();

            // Act
            var result = await client.DeleteAsync(DeleteRoute);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
