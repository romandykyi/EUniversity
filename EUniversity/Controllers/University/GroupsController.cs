using EUniversity.Core.Dtos.University;
using EUniversity.Core.Filters;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using EUniversity.Core.Services.University;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using System.Diagnostics;

namespace EUniversity.Controllers.University;

/// <summary>
/// Groups controller.
/// </summary>
[ApiController]
[Route("api/groups")]
[FluentValidationAutoValidation]
public class GroupsController : ControllerBase
{
    private readonly IGroupsService _groupsService;
    private readonly IStudentGroupsService _studentGroupsService;
    private readonly IEntityExistenceChecker _existenceChecker;

    public GroupsController(IGroupsService groupsService,
        IStudentGroupsService studentGroupsService,
        IEntityExistenceChecker existenceChecker)
    {
        _groupsService = groupsService;
        _studentGroupsService = studentGroupsService;
        _existenceChecker = existenceChecker;
    }

    /// <summary>
    /// Gets a page with groups.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// </remarks>
    /// <param name="properties">Pagination properties.</param>
    /// <param name="name">An optional name to filter groups by.</param>
    /// <response code="200">Returns requested page with groups.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    [HttpGet]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(Page<GroupViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGroupsPageAsync(
        [FromQuery] PaginationProperties properties,
        [FromQuery] string? name)
    {
        NameFilter<Group>? filter = name != null ? new(name) : null;
        return Ok(await _groupsService.GetPageAsync(properties, filter));
    }

    /// <summary>
    /// Gets a group by its ID.
    /// </summary>
    /// <response code="200">Returns requested group</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="404">Group does not exist</response>
    [HttpGet("{id:int}", Name = nameof(GetGroupByIdAsync))]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(GroupViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGroupByIdAsync([FromRoute] int id)
    {
        var group = await _groupsService.GetByIdAsync(id);
        return group != null ? Ok(group) : NotFound();
    }

    /// <summary>
    /// Creates a new group.
    /// </summary>
    /// <response code="201">Successfully created</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpPost]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(typeof(GroupViewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateGroupAsync([FromBody] GroupCreateDto dto)
    {
        var group = await _groupsService.CreateAsync(dto);
        var routeValues = new { id = group.Id };
        var body = group.Adapt<GroupViewDto>();
        return CreatedAtRoute(nameof(GetGroupByIdAsync), routeValues, body);
    }

    /// <summary>
    /// Edits a group by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Group does not exist</response>
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGroupAsync([FromRoute] int id,
        [FromBody] GroupCreateDto dto)
    {
        var result = await _groupsService.UpdateAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes a group by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Group does not exist</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGroupAsync([FromRoute] int id)
    {
        var result = await _groupsService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }

    private IActionResult CustomNotFound(string message)
    {
        return NotFound(new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            title = "Resource not found",
            status = StatusCodes.Status404NotFound,
            traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            message
        });
    }

    /// <summary>
    /// Adds a student to a group.
    /// </summary>
    /// <response code="200">Success(user is already part of the group)</response>
    /// <response code="201">Success(user was added to the group)</response>
    /// <response code="400">Malformed input, or invalid user ID, or user is not a student</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Group does not exist</response>
    [HttpPost]
    [Route("{groupId:int}/students")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddStudentToGroupAsync(
        [FromRoute] int groupId, [FromBody] AssignStudentDto dto)
    {
        if (!await _existenceChecker.ExistsAsync<Group, int>(groupId))
        {
            return CustomNotFound("The group with the specified ID does not exist.");
        }
        bool created = await _studentGroupsService.AssignAsync(groupId, dto.StudentId);

        if (created)
        {
            var routeValues = new { groupId, studentId = dto.StudentId };
            return CreatedAtRoute(nameof(DeleteStudentFromGroupAsync), routeValues, null);
        }
        return Ok(new { message = "The student is already part of the group" });

    }

    /// <summary>
    /// Deletes a student from a group.
    /// </summary>
    /// <response code="204">Success(user was removed from the group)</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Group/Student does not exist or student is not part of the group</response>
    [HttpDelete]
    [Route("{groupId:int}/students/{studentId}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStudentFromGroupAsync(
        [FromRoute] int groupId, [FromRoute] string studentId)
    {
        if (!await _existenceChecker.ExistsAsync<Group, int>(groupId))
        {
            return CustomNotFound("The group with the specified ID does not exist.");
        }
        if (!await _existenceChecker.ExistsAsync<ApplicationUser, string>(studentId))
        {
            return CustomNotFound("The user with the specified ID does not exist.");
        }

        bool removed = await _studentGroupsService.UnassignAsync(groupId, studentId);
        if (removed)
        {
            return NoContent();
        }
        return CustomNotFound("The user is not part of the group.");
    }
}
