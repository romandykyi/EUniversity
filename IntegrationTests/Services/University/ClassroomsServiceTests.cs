using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Infrastructure.Services.University;

namespace EUniversity.IntegrationTests.Services.University
{
    public class ClassroomsServiceTests : CrudServicesTest
        <IClassroomsService, Classroom, int, ViewClassroomDto, ViewClassroomDto, CreateClassroomDto, CreateClassroomDto>
    {
        /// <inheritdoc />
        protected override void AssertThatWasUpdated(Classroom actualEntity, CreateClassroomDto updateDto)
        {
            Assert.That(actualEntity.Name, Is.EqualTo(updateDto.Name));
        }

        /// <inheritdoc />
        protected override int GetNonExistentId() => -1;

        /// <inheritdoc />
        protected override Classroom GetTestEntity()
        {
            return new()
            {
                Name = "#100"
            };
        }

        /// <inheritdoc />
        protected override CreateClassroomDto GetValidCreateDto()
        {
            return new("#200");
        }

        /// <inheritdoc />
        protected override CreateClassroomDto GetValidUpdateDto()
        {
            return new("#300");
        }
    }
}
