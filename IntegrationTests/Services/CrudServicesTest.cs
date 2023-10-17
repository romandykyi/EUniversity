using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EUniversity.IntegrationTests.Services
{
    /// <summary>
    /// Class that implements base CRUD service tests.
    /// </summary>
    public abstract class CrudServicesTest<TService, TEntity, TId, TDetailsDto, TCreateDto, TUpdateDto>
         : ServicesTest
        where TService : ICrudService<TEntity, TId, TDetailsDto, TCreateDto, TUpdateDto>
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
    {
        protected TService Service { get; private set; }

        [SetUp]
        public virtual void SetUpService()
        {
            Service = ServiceScope.ServiceProvider.GetService<TService>()!;
        }

        /// <summary>
        /// Check whether an entity with <paramref name="id"/> exists.
        /// </summary>
        /// <param name="id">ID of the entity.</param>
        /// <returns>
        /// <see langword="true" /> if entity exists;
        /// <see langword="false" /> otherwise.
        /// </returns>
        protected async Task<bool> EntityExistsAsync(TId id)
        {
            return await DbContext.Set<Classroom>().AnyAsync(x => x.Id.Equals(id));
        }

        /// <summary>
        /// When implemented, gets a test entity that can be added to the DB.
        /// </summary>
        /// <returns>
        /// Entity that can be added to the DB.
        /// </returns>
        protected abstract TEntity GetTestEntity();

        /// <summary>
        /// When implemented, gets valid creation DTO.
        /// </summary>
        /// <returns>
        /// Valid DTO used for creation.
        /// </returns>
        protected abstract TCreateDto GetValidCreateDto();

        /// <summary>
        /// When implemented, gets valid update/edit DTO.
        /// </summary>
        /// <returns>
        /// Valid DTO used for update/edit.
        /// </returns>
        protected abstract TUpdateDto GetValidUpdateDto();

        /// <summary>
        /// Gets the identifier that represents a non-existent entity.
        /// </summary>
        /// <returns>
        /// The identifier for a non-existent entity.
        /// </returns>
        protected abstract TId GetNonExistentId();

        /// <summary>
        /// Creates test entity and adds it to the DB.
        /// </summary>
        /// <returns>
        /// <see cref="TEntity" /> that was created.
        /// </returns>
        protected virtual async Task<TEntity> CreateTestEntityAsync()
        {
            var entity = GetTestEntity();
            DbContext.Add(entity);
            await DbContext.SaveChangesAsync();

            return entity;
        }

        /// <summary>
        /// When implemented, asserts that entity was actually updated.
        /// </summary>
        /// <param name="actualEntity">Entity, that should be updated.</param>
        /// <param name="updateDto">Update DTO, that was used to update entity.</param>
        protected abstract void AssertThatWasUpdated(TEntity actualEntity, TUpdateDto updateDto);

        [Test]
        public virtual async Task GetById_ElementExists_ReturnsValidElement()
        {
            // Arrange
            var classroom = await CreateTestEntityAsync();
            var expectedResult = classroom.Adapt<TDetailsDto>();

            // Act
            var result = await Service.GetByIdAsync(classroom.Id);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public virtual async Task GetById_ElementDoesNotExist_ReturnsNull()
        {
            // Act
            var result = await Service.GetByIdAsync(GetNonExistentId());

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public virtual async Task Create_ValidInput_CreatesElement()
        {
            // Arrange
            TCreateDto dto = GetValidCreateDto();

            // Act
            TId id = await Service.CreateAsync(dto);

            // Assert(check if element is actually created)
            Assert.That(await EntityExistsAsync(id));
        }

        [Test]
        public virtual async Task Update_ElementExists_Succeeds()
        {
            // Arrange
            var entity = await CreateTestEntityAsync();
            TUpdateDto dto = GetValidUpdateDto();
            var expectedClassroom = dto.Adapt<Classroom>();

            // Act
            bool result = await Service.UpdateAsync(entity.Id, dto);

            // Assert
            Assert.That(result);
            var actualEntity = await DbContext.Set<TEntity>()
                .FirstOrDefaultAsync(c => c.Id.Equals(entity.Id));
            // Check if element was updated
            Assert.That(actualEntity, Is.Not.Null);
            AssertThatWasUpdated(actualEntity, dto);
        }

        [Test]
        public virtual async Task Update_ElementDoesNotExist_ReturnFalse()
        {
            // Act
            bool result = await Service.UpdateAsync(GetNonExistentId(), GetValidUpdateDto());

            // Arrange
            Assert.That(result, Is.False);
        }

        [Test]
        public virtual async Task Delete_ElementExists_Succeeds()
        {
            // Arrange
            var classroom = await CreateTestEntityAsync();

            // Act
            bool result = await Service.DeleteAsync(classroom.Id);

            // Assert
            Assert.Multiple(async () =>
            {
                Assert.That(result);
                Assert.That(await EntityExistsAsync(classroom.Id), Is.False);
            });
        }

        [Test]
        public virtual async Task Delete_ElementDoesNotExist_ReturnsFalse()
        {
            // Act
            bool result = await Service.DeleteAsync(GetNonExistentId());

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
