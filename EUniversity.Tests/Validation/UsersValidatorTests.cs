using EUniversity.Core.Models;
using EUniversity.Core.Policy;
using Microsoft.AspNetCore.Identity;
using NSubstitute.ReturnsExtensions;

namespace EUniversity.Tests.Validation;

/// <summary>
/// Abstract class that provides mocked <see cref="UserManager{ApplicationUser}" />
/// which can be used for testing validators which validate users roles.
/// </summary>
public abstract class UsersValidatorTests
{
    protected UserManager<ApplicationUser> UserManagerMock { get; private set; }

    /// <summary>
    /// ID of the user which has <see cref="Roles.Teacher" /> role.
    /// </summary>
    public const string TestTeacherId = "test-teacher-id";
    /// <summary>
    /// ID of the user which has <see cref="Roles.Student" /> role.
    /// </summary>
    public const string TestStudentId = "test-student-id";
    /// <summary>
    /// ID of the user which has no roles.
    /// </summary>
    public const string TestUserId = "test-user-id";
    /// <summary>
    /// ID of the non-existent user.
    /// </summary>
    public const string NonExistentUserId = "invalid";

    [OneTimeSetUp]
    public void SetUpUserManagerMock()
    {
        var userStore = Substitute.For<IUserStore<ApplicationUser>>();
        UserManagerMock = Substitute.For<UserManager<ApplicationUser>>(
            userStore, null, null, null, null, null, null, null, null
            );

        UserManagerMock
            .FindByIdAsync(Arg.Any<string>())
            .ReturnsNull();
        UserManagerMock
            .FindByIdAsync(TestTeacherId)
            .Returns(new ApplicationUser() { Id = TestTeacherId });
        UserManagerMock
            .FindByIdAsync(TestStudentId)
            .Returns(new ApplicationUser() { Id = TestStudentId });
        UserManagerMock
            .FindByIdAsync(TestUserId)
            .Returns(new ApplicationUser() { Id = TestUserId });

        UserManagerMock
            .IsInRoleAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
            .Returns(false);
        UserManagerMock
            .IsInRoleAsync(Arg.Is<ApplicationUser>(u => u.Id == TestTeacherId), Roles.Teacher)
            .Returns(true);
        UserManagerMock
            .IsInRoleAsync(Arg.Is<ApplicationUser>(u => u.Id == TestStudentId), Roles.Student)
            .Returns(true);
    }

    [OneTimeTearDown]
    public void TearDownUserManagerMock()
    {
        UserManagerMock.Dispose();
    }
}
