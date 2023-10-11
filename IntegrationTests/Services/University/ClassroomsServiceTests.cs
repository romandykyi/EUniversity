using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Infrastructure.Services.University;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EUniversity.IntegrationTests.Services.University
{
    public class ClassroomsServiceTests : ServicesTest
    {
        private IClassroomsService _classroomsService;
        public static readonly CreateClassromDto
            DefaultCreateClassroomDto = new("new classroom");

        private async Task<Classroom> CreateDefaultClassroomAsync()
        {
            Classroom classroom = new()
            {
                Name = "100"
            };
            DbContext.Add(classroom);
            await DbContext.SaveChangesAsync();

            return classroom;
        }

        private async Task<bool> ClassroomExists(int id)
        {
            return await DbContext.Set<Classroom>().AnyAsync(x => x.Id == id);
        }

        [SetUp]
        public void SetUp()
        {
            _classroomsService = ServiceScope.ServiceProvider.GetService<IClassroomsService>()!;
        }

        [Test]
        public async Task GetById_ElementExists_ReturnsValidElement()
        {
            // Arrange
            var classroom = await CreateDefaultClassroomAsync();
            var expectedResult = classroom.Adapt<ViewClassroomDto>();

            // Act
            var result = await _classroomsService.GetByIdAsync(classroom.Id);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetById_ElementDoesNotExist_ReturnsNull()
        {
            // Act
            var result = await _classroomsService.GetByIdAsync(-1);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Create_ValidInput_CreatesElement()
        {
            // Arrange
            CreateClassromDto dto = new("Classroom Name");

            // Act
            int id = await _classroomsService.CreateAsync(dto);

            // Assert(check if element is actually created)
            Assert.That(await ClassroomExists(id));
        }

        [Test]
        public async Task Update_ElementExists_Succeeds()
        {
            // Arrange
            var classroom = await CreateDefaultClassroomAsync();
            CreateClassromDto dto = new(classroom.Name + "2");
            var expectedClassroom = dto.Adapt<Classroom>();

            // Act
            bool result = await _classroomsService.UpdateAsync(classroom.Id, dto);

            // Assert
            Assert.That(result);
            var actualElement = await DbContext.Set<Classroom>()
                .FirstOrDefaultAsync(c => c.Id == classroom.Id);
            // Check if element was updated
            Assert.That(actualElement, Is.Not.Null);
            Assert.That(actualElement.Name, Is.EqualTo(dto.Name));
        }

        [Test]
        public async Task Update_ElementDoesNotExist_ReturnFalse()
        {
            // Act
            bool result = await _classroomsService.UpdateAsync(-1, DefaultCreateClassroomDto);

            // Arrange
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task Delete_ElementExists_Succeeds()
        {
            // Arrange
            var classroom = await CreateDefaultClassroomAsync();

            // Act
            bool result = await _classroomsService.DeleteAsync(classroom.Id);

            // Assert
            Assert.That(result);
            Assert.That(await ClassroomExists(classroom.Id), Is.False);
        }

        [Test]
        public async Task Delete_ElementDoesNotExist_ReturnsFalse()
        {
            // Act
            bool result = await _classroomsService.DeleteAsync(-1);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
