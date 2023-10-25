using EUniversity.Core.Models.University;
using EUniversity.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EUniversity.IntegrationTests.Services;

public class EntityExistenceCheckerTest : ServicesTest
{
    private IEntityExistenceChecker _entityExistenceChecker;

    [SetUp]
    public void SetUpService()
    {
        _entityExistenceChecker = ServiceScope
            .ServiceProvider.GetService<IEntityExistenceChecker>()!;
    }

    [Test]
    public async Task ExistsAsync_ClassroomExists_ReturnsTrue()
    {
        // Arrange
        Classroom classroom = new() { Name = "100" };
        DbContext.Add(classroom);
        await DbContext.SaveChangesAsync();

        // Act
        bool result = await _entityExistenceChecker
            .ExistsAsync<Classroom, int>(classroom.Id);

        // Assert
        Assert.That(result);
    }

    [Test]
    public async Task ExistsAsync_InvalidId_ReturnsFalse()
    {
        // Arrange
        const int id = -1;

        // Act
        bool result = await _entityExistenceChecker.ExistsAsync<Classroom, int>(id);

        // Assert
        Assert.That(result, Is.False);
    }
}
