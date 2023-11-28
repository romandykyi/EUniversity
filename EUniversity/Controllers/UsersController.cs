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
[Authorize(Policies.HasAdministratorPermission)]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUsersService _usersService;

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
    [ProducesResponseType(typeof(Page<UserViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpGet]
    [Route("students")]
    [ProducesResponseType(typeof(Page<UserViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpGet]
    [Route("teachers")]
    [ProducesResponseType(typeof(Page<UserViewDto>), StatusCodes.Status200OK)]
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
    /// <response code="201">Returns status of students</response>
    /// <response code="400">Invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpPost]
    [Route("students")]
    [ProducesResponseType(typeof(IEnumerable<CreatedUserDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RegisterStudentsAsync([FromBody] RegisterUsersDto students)
    {
        return await RegisterAsync(students, Roles.Student, "api/users/students");
    }

    /// <summary>
    /// Registers teachers.
    /// </summary>
    /// <response code="201">Returns status of teachers</response>
    /// <response code="400">Invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpPost]
    [Route("teachers")]
    [ProducesResponseType(typeof(IEnumerable<CreatedUserDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RegisterTeachersAsync([FromBody] RegisterUsersDto teachers)
    {
        return await RegisterAsync(teachers, Roles.Teacher, "api/users/teachers");
    }
    #endregion
}
