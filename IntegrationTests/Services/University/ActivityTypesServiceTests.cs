using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;

namespace EUniversity.IntegrationTests.Services.University;

public class ActivityTypesServiceTests : CrudServicesTest
    <IActivityTypesService, ActivityType, int, ActivityTypeViewDto, ActivityTypeViewDto, ActivityTypeCreateDto, ActivityTypeCreateDto>
{
    public static ActivityType CreateTestActivityType()
    {
        return new()
        {
            Name = "Exam"
        };
    }

    /// <inheritdoc />
    protected override void AssertThatWasUpdated(ActivityType actualEntity, ActivityTypeCreateDto updateDto)
    {
        Assert.That(actualEntity.Name, Is.EqualTo(updateDto.Name));
    }

    /// <inheritdoc />
    protected override int GetNonExistentId() => -1;

    /// <inheritdoc />
    protected override ActivityType GetTestEntity()
    {
        return CreateTestActivityType();
    }

    /// <inheritdoc />
    protected override ActivityTypeCreateDto GetValidCreateDto()
    {
        return new("Test");
    }

    /// <inheritdoc />
    protected override ActivityTypeCreateDto GetValidUpdateDto()
    {
        return new("Presentation");
    }
}
