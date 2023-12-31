﻿using EUniversity.Core.Dtos.University;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Filters;
using EUniversity.Core.Models.University;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Filters;

namespace EUniversity.IntegrationTests.Controllers.University;

public class GroupsControllerTests :
    AdminCrudControllersTest<Group, int, GroupPreviewDto, GroupViewDto, GroupCreateDto, GroupCreateDto>
{
    public readonly TeacherPreviewDto TeacherPreviewDto = new(Guid.NewGuid().ToString(), "test-teacher", "Teacher1", "Teacher2", null);
    public readonly CoursePreviewDto CoursePreviewDto = new(5, "Some Course", DateTimeOffset.Now, DateTimeOffset.Now, null);

    public override string GetPageRoute => "api/groups";

    public override string GetByIdRoute => $"api/groups/{DefaultId}";

    public override string PostRoute => "api/groups";

    public override string PutRoute => $"api/groups/{DefaultId}";

    public override string DeleteRoute => $"api/groups/{DefaultId}";

    public override int DefaultId => 1;

    public override string GetPageFilter => "sortingMode=name&semesterId=0&courseId=12";

    protected override void AssertThatViewDtosAreEqual(GroupViewDto expected, GroupViewDto actual)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.Course, Is.EqualTo(expected.Course));
            Assert.That(actual.Teacher, Is.EqualTo(expected.Teacher));
        });
    }

    public override void SetUpService()
    {
        ServiceMock = WebApplicationFactory.GroupsServiceMock;
        Assert.That(ServiceMock, Is.InstanceOf<IGroupsService>());
        SetUpValidationMocks();
    }

    protected override GroupCreateDto GetInvalidCreateDto()
    {
        return new(string.Empty, DefaultId, null);
    }

    protected override GroupCreateDto GetInvalidUpdateDto()
    {
        return GetInvalidCreateDto();
    }

    protected override GroupViewDto GetTestDetailsDto()
    {
        return new(DefaultId, "Group",
            DateTimeOffset.Now, DateTimeOffset.Now,
            TeacherPreviewDto,
            CoursePreviewDto);
    }

    protected override GroupPreviewDto GetTestPreviewDto()
    {
        return new(DefaultId, "Group",
            DateTimeOffset.Now, DateTimeOffset.Now,
            TeacherPreviewDto, CoursePreviewDto);
    }

    protected override GroupCreateDto GetValidCreateDto()
    {
        return new("Group", 5, "TeacherID");
    }

    protected override GroupCreateDto GetValidUpdateDto()
    {
        return new("Group2", 5, null);
    }

    protected override bool AssertThatFilterWasApplied(IFilter<Group> filter)
    {
        return filter is GroupsFilter groupsFilter &&
            groupsFilter.Properties.SemesterId == 0 &&
            groupsFilter.Properties.CourseId == 12 &&
            groupsFilter.SortingMode == DefaultFilterSortingMode.Name;
    }
}
