﻿using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Infrastructure.Services.University;

namespace EUniversity.IntegrationTests.Services.University
{
    public class ClassroomsServiceTests : CrudServicesTest
        <IClassroomsService, Classroom, int, ClassroomViewDto, ClassroomViewDto, ClassroomCreateDto, ClassroomCreateDto>
    {
        /// <inheritdoc />
        protected override void AssertThatWasUpdated(Classroom actualEntity, ClassroomCreateDto updateDto)
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
        protected override ClassroomCreateDto GetValidCreateDto()
        {
            return new("#200");
        }

        /// <inheritdoc />
        protected override ClassroomCreateDto GetValidUpdateDto()
        {
            return new("#300");
        }
    }
}
