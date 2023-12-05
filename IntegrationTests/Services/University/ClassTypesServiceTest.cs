using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;

namespace EUniversity.IntegrationTests.Services.University;

public class ClassTypesServiceTests : CrudServicesTest
    <IClassTypesService, ClassType, int, ClassTypeViewDto, ClassTypeViewDto, ClassTypeCreateDto, ClassTypeCreateDto>
{
    /// <inheritdoc />
    protected override void AssertThatWasUpdated(ClassType actualEntity, ClassTypeCreateDto updateDto)
    {
        Assert.That(actualEntity.Name, Is.EqualTo(updateDto.Name));
    }

    /// <inheritdoc />
    protected override int GetNonExistentId() => -1;

    /// <inheritdoc />
    protected override ClassType GetTestEntity()
    {
        return new ClassType { Name = "Lecture" };
    }

    /// <inheritdoc />
    protected override ClassTypeCreateDto GetValidCreateDto()
    {
        return new("Practical");
    }

    /// <inheritdoc />
    protected override ClassTypeCreateDto GetValidUpdateDto()
    {
        return new("Laboratory");
    }
}
