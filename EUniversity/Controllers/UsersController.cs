using EUniversity.Core.Dtos.University;
using EUniversity.Core.Dtos.Users;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.Auth;
using EUniversity.Core.Services.Users;
using EUniversity.Infrastructure.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace EUniversity.Controllers;

[ApiController]
[Route("api/users")]
[FluentValidationAutoValidation]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUsersService _usersService;

    public const string StudentIdRouteKey = "studentId";

    public UsersController(IAuthService authService, IUsersService usersService)
    {
        _authService = authService;
        _usersService = usersService;
    }

    #region Get
    /// <summary>
    /// Gets a page with users.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// <br />
    /// 'sortingMode' is an optional query param that accepts one of these values
    /// <ul>
    /// <li>default(or 0) - no sorting will be applied;</li>
    /// <li>fullName(or 1) - users will be sorted by their full name(from a to z), this mode is applied by default;</li>
    /// <li>fullNameDescending(or 2) - users will be sorted by their full name in descending order(from z to a);</li>
    /// <li>username(or 3) - users will be sorted by their username(from a to z), this mode is applied by default;</li>
    /// <li>usernameDescending(or 4) - users will be sorted by their username in descending order(from z to a).</li>
    /// </ul>
    /// </remarks>
    /// <response code="200">Returns a page with users</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpGet]
    [ProducesResponseType(typeof(Page<UserPreviewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policies.HasAdministratorPermission)]
    public async Task<IActionResult> GetAllUsersAsync(
        [FromQuery] PaginationProperties paginationProperties,
        [FromQuery] UsersFilterProperties usersFilter)
    {
        return Ok(await _usersService.GetAllUsersAsync(paginationProperties,
            new UsersFilter(usersFilter)));
    }

    /// <summary>
    /// Gets a page with students.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// <br />
    /// 'sortingMode' is an optional query param that accepts one of these values
    /// <ul>
    /// <li>default(or 0) - no sorting will be applied;</li>
    /// <li>fullName(or 1) - students will be sorted by their full name(from a to z), this mode is applied by default;</li>
    /// <li>fullNameDescending(or 2) - students will be sorted by their full name in descending order(from z to a);</li>
    /// <li>username(or 3) - students will be sorted by their username(from a to z), this mode is applied by default;</li>
    /// <li>usernameDescending(or 4) - students will be sorted by their username in descending order(from z to a).</li>
    /// </ul>
    /// </remarks>
    /// <response code="200">Returns a page with students</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' or 'Teacher' role</response>
    [HttpGet]
    [Route("students")]
    [ProducesResponseType(typeof(Page<UserPreviewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policies.IsTeacherOrAdministrator)]
    public async Task<IActionResult> GetAllStudentsAsync(
        [FromQuery] PaginationProperties paginationProperties,
        [FromQuery] UsersFilterProperties usersFilter)
    {
        return Ok(await _usersService.GetUsersInRoleAsync(Roles.Student,
            paginationProperties, new UsersFilter(usersFilter)));
    }

    /// <summary>
    /// Gets a page with teachers.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// <br />
    /// 'sortingMode' is an optional query param that accepts one of these values
    /// <ul>
    /// <li>default(or 0) - no sorting will be applied;</li>
    /// <li>fullName(or 1) - teachers will be sorted by their full name(from a to z), this mode is applied by default;</li>
    /// <li>fullNameDescending(or 2) - teachers will be sorted by their full name in descending order(from z to a);</li>
    /// <li>username(or 3) - teachers will be sorted by their username(from a to z), this mode is applied by default;</li>
    /// <li>usernameDescending(or 4) - teachers will be sorted by their username in descending order(from z to a).</li>
    /// </ul>
    /// </remarks>
    /// <response code="200">Returns a page with teachers</response>
    /// <response code="401">Unauthorized user call</response>
    [HttpGet]
    [Route("teachers")]
    [ProducesResponseType(typeof(Page<UserPreviewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllTeachersAsync(
        [FromQuery] PaginationProperties paginationProperties,
        [FromQuery] UsersFilterProperties usersFilter)
    {
        return Ok(await _usersService.GetUsersInRoleAsync(Roles.Teacher,
            paginationProperties, new UsersFilter(usersFilter)));
    }
    #endregion

    #region Post
    private async Task<IActionResult> RegisterAsync(RegisterUsersDto students, string role, string location)
    {
        List<CreatedUserDto> createdUsers = new();
        await foreach (var result in _authService.RegisterManyAsync(students.Users, role))
        {
            CreatedUserDto createdUser = new(
                result.Id,
                result.UserName,
                result.Password,
                result.Succeeded,
                result.Succeeded ? null : result.Errors.Select(e => e.Description)
            );
            createdUsers.Add(createdUser);
        }

        return Created(location, createdUsers);
    }

    /// <summary>
    /// Registers students.
    /// </summary>
    /// <response code="201">
    /// Student accounts were attempted to be registered. 
    /// Successfully registered students have the 'success' property is set to 
    /// true and their credentials are returned. If a student could not be registered,
    /// then the 'success' property is set to false and an array of error messages is returned.
    /// </response>
    /// <response code="400">Invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpPost]
    [Route("students")]
    [ProducesResponseType(typeof(IEnumerable<CreatedUserDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policies.HasAdministratorPermission)]
    public async Task<IActionResult> RegisterStudentsAsync([FromBody] RegisterUsersDto students)
    {
        return await RegisterAsync(students, Roles.Student, "api/users/students");
    }

    /// <summary>
    /// Registers teachers.
    /// </summary>
    /// <response code="201">
    /// Teacher accounts were attempted to be registered. 
    /// Successfully registered teachers have the 'success' property is set to 
    /// true and their credentials are returned. If a teacher could not be registered,
    /// then the 'success' property is set to false and an array of error messages is returned.
    /// </response>
    /// <response code="400">Invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpPost]
    [Route("teachers")]
    [ProducesResponseType(typeof(IEnumerable<CreatedUserDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Policies.HasAdministratorPermission)]
    public async Task<IActionResult> RegisterTeachersAsync([FromBody] RegisterUsersDto teachers)
    {
        return await RegisterAsync(teachers, Roles.Teacher, "api/users/teachers");
    }
    #endregion

    #region Enrollments
    /// <summary>
    /// Gets a page with groups of the student.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If a user has the 'Administrator' or 'Teacher' role then he/she can use this method
    /// for every user, otherwise a user can view only his/her own groups.
    /// </para>
    /// <para>
    /// The 'studentId' route value is not checked in this method, if student with
    /// this ID does not exist, then empty page will be returned.
    /// </para>
    /// <para>
    /// If there is no items in the requested page, then empty page will be returned.
    /// </para>
    /// <para>
    /// If the query param 'semesterId' is 0, then groups that are not linked to any semesters will be returned.</para>
    /// </remarks>
    /// <param name="studentId">ID of the student whose groups will be returned.</param>
    /// <param name="properties">Pagination properties.</param>
    /// <param name="filterProperties">Filter properties.</param>
    /// <param name="name">An optional name to filter groups by.</param>
    /// <param name="sortingMode">
    /// An optional sorting mode.
    /// <para>
    /// Possible values:
    /// </para>
    /// <ul>
    /// <li>default(or 0) - no sorting will be applied;</li>
    /// <li>name(or 1) - groups will be sorted by their name(from a to z), this mode is applied by default;</li>
    /// <li>nameDescending(or 2) - groups will be sorted by their name in descending order(from z to a);</li>
    /// <li>newest(or 3) - groups will be sorted by their creation date in descending order;</li>
    /// <li>oldest(or 4) - groups will be sorted by their creation date in ascending order.</li>
    /// </ul>
    /// </param>
    /// <response code="200">Returns requested page with groups.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="401">Unauthorized user call.</response>
    /// <response code="403">User don't have a permission to view specified student's enrollments.</response>
    [HttpGet("students/{studentId}/groups")]
    [Authorize(Policies.CanViewStudentEnrollments)]
    [ProducesResponseType(typeof(Page<GroupPreviewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetStudentGroupsAsync(
        [FromRoute] string studentId,
        [FromQuery] PaginationProperties properties,
        [FromQuery] GroupsFilterProperties filterProperties,
        [FromQuery] string? name,
        [FromQuery] DefaultFilterSortingMode sortingMode = DefaultFilterSortingMode.Name)
    {
        GroupsFilter filter = new(filterProperties, name ?? string.Empty, sortingMode);
        return Ok(await _usersService.GetGroupsOfStudentAsync(studentId, properties, filter));
    }

    /// <summary>
    /// Gets a page with semesters of the student.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If a user has the 'Administrator' or 'Teacher' role then he/she can use this method
    /// for every user, otherwise a user can view only his/her own semesters.
    /// </para>
    /// <para>
    /// The 'studentId' route value is not checked in this method, if student with
    /// this ID does not exist, then empty page will be returned.
    /// </para>
    /// <para>
    /// If there is no items in the requested page, then empty page will be returned.
    /// </para>
    /// <para>
    /// If the query param 'semesterId' is 0, then semesters that are not linked to any semesters will be returned.</para>
    /// </remarks>
    /// <param name="studentId">ID of the student whose semesters will be returned.</param>
    /// <param name="properties">Pagination properties.</param>
    /// <param name="filterProperties">Filter properties.</param>
    /// <param name="name">An optional name to filter semesters by.</param>
    /// <param name="sortingMode">
    /// An optional sorting mode.
    /// <para>
    /// Possible values:
    /// </para>
    /// <ul>
    /// <li>default(or 0) - no sorting will be applied;</li>
    /// <li>name(or 1) - semesters will be sorted by their name(from a to z), this mode is applied by default;</li>
    /// <li>nameDescending(or 2) - semesters will be sorted by their name in descending order(from z to a);</li>
    /// <li>newest(or 3) - semesters will be sorted by their creation date in descending order;</li>
    /// <li>oldest(or 4) - semesters will be sorted by their creation date in ascending order.</li>
    /// </ul>
    /// </param>
    /// <response code="200">Returns requested page with semesters.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="401">Unauthorized user call.</response>
    /// <response code="403">User don't have a permission to view specified student's enrollments.</response>
    [HttpGet("students/{studentId}/semesters")]
    [Authorize(Policies.CanViewStudentEnrollments)]
    [ProducesResponseType(typeof(Page<SemesterPreviewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetStudentSemestersAsync(
        [FromRoute] string studentId,
        [FromQuery] PaginationProperties properties,
        [FromQuery] SemestersFilterProperties filterProperties,
        [FromQuery] string? name,
        [FromQuery] DefaultFilterSortingMode sortingMode = DefaultFilterSortingMode.Name)
    {
        SemestersFilter filter = new(filterProperties, name ?? string.Empty, sortingMode);
        return Ok(await _usersService.GetSemestersOfStudentAsync(studentId, properties, filter));
    }
    #endregion
}
