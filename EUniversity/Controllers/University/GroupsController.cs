using Duende.IdentityServer.Extensions;
using EUniversity.Core.Dtos.University;
using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using EUniversity.Core.Services.University;
using EUniversity.Core.Services.University.Grades;
using EUniversity.Infrastructure.Filters;
using IdentityModel;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

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
    private readonly IAssignedGradesService _assignedGradesService;

    public GroupsController(IGroupsService groupsService,
        IStudentGroupsService studentGroupsService,
        IEntityExistenceChecker existenceChecker,
        IAssignedGradesService assignedGradesService)
    {
        _groupsService = groupsService;
        _studentGroupsService = studentGroupsService;
        _existenceChecker = existenceChecker;
        _assignedGradesService = assignedGradesService;
    }

    /// <summary>
    /// Gets a page with groups.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// If the query param 'semesterId' is 0, then groups that are not linked to any semesters will be returned.
    /// </remarks>
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
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    [HttpGet]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(Page<GroupViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGroupsPageAsync(
        [FromQuery] PaginationProperties properties,
        [FromQuery] GroupsFilterProperties filterProperties,
        [FromQuery] string? name,
        [FromQuery] DefaultFilterSortingMode sortingMode = DefaultFilterSortingMode.Name)
    {
        GroupsFilter filter = new(filterProperties, name ?? string.Empty, sortingMode);
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

    /// <summary>
    /// Gets a page with all students that are part of the group with the given ID.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// <br />
    /// 'sortingMode' is an optional query param that accepts one of these values
    /// <ul>
    /// <li>default(or 0) - no sorting will be applied;</li>
    /// <li>fullName(or 1) - enrollments will be sorted by student's full name(from a to z), this mode is applied by default;</li>
    /// <li>fullNameDescending(or 2) - enrollments will be sorted by student's full name in descending order(from z to a);</li>
    /// <li>newest(or 3) - enrollments will be sorted by their date in descending order;</li>
    /// <li>oldest(or 4) - enrollments will be sorted by their date in ascending order.</li>
    /// </ul>
    /// </remarks>
    /// <response code="200">Returns requested page with students that are part of the group.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">Caller is not a teacher or an administrator</response>
    /// <response code="404">Group does not exist</response>
    [HttpGet]
    [Route("{groupId:int}/students")]
    [Authorize(Policies.IsTeacherOrAdministrator)]
    [ProducesResponseType(typeof(Page<StudentGroupViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentsInGroupAsync(
        [FromRoute] int groupId,
        [FromQuery] PaginationProperties properties,
        [FromQuery] StudentEnrollmentsFilterProperties filterProperties)
    {
        if (!await _existenceChecker.ExistsAsync<Group, int>(groupId))
        {
            return NotFound(
                CustomResponses.NotFound("The group with the specified ID does not exist.",
                HttpContext));
        }
        StudentEnrollmentsFilter<StudentGroup> filter = new(filterProperties);
        return Ok(await _studentGroupsService
            .GetAssigningEntitiesPageAsync<StudentGroupViewDto>(groupId, properties, filter));
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
            return NotFound(
                CustomResponses.NotFound("The group with the specified ID does not exist.",
                HttpContext));
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
            return NotFound(
                CustomResponses.NotFound("The group with the specified ID does not exist.",
                HttpContext));
        }
        if (!await _existenceChecker.ExistsAsync<ApplicationUser, string>(studentId))
        {
            return NotFound(
                CustomResponses.NotFound("The user with the specified ID does not exist.",
                HttpContext));
        }

        bool removed = await _studentGroupsService.UnassignAsync(groupId, studentId);
        if (removed)
        {
            return NoContent();
        }
        return NotFound(
                CustomResponses.NotFound("The user is not part of the group.",
                HttpContext));
    }

    /// <summary>
    /// Gets a page with all grades assigned in the group.
    /// </summary>
    /// <remarks>
    /// Only the group's owner or an administrator can access this method.
    /// <br />
    /// If there is no items in the requested page, then empty page will be returned.
    /// </remarks>
    /// <response code="200">Returns requested page with grades assigned in the group.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role or not an owner of the group</response>
    /// <response code="404">Group does not exist</response>
    [HttpGet]
    [Route("{groupId:int}/grades")]
    [Authorize(Policies.IsTeacherOrAdministrator)]
    [ProducesResponseType(typeof(Page<StudentGroupViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGradesInGroupAsync(
        [FromRoute] int groupId,
        [FromQuery] PaginationProperties properties,
        [FromQuery] AssignedGradesFilterProperties filterProperties)
    {
        string callerId = User.Identity.GetSubjectId();
        var response = await _groupsService.GetOwnerIdAsync(groupId);
        if (!response.GroupExists)
        {
            return NotFound(
                CustomResponses.NotFound("The group with the specified ID does not exist.",
                HttpContext));
        }
        // Forbid if user is not an administrator and doesn't own the group
        if (response.OwnerId != callerId && 
            !User.HasClaim(JwtClaimTypes.Role, Roles.Administrator))
        {
            return Forbid();
        }
        AssignedGradesFilter filter = new(filterProperties, groupId: groupId);
        return Ok(await _assignedGradesService
            .GetPageAsync<AssignedGradeViewDto>(properties, filter, includeGroups: false));
    }

    /// <summary>
    /// Gets a page with all grades assigned in the group to the student.
    /// </summary>
    /// <remarks>
    /// Only the group's owner, an administrator or grades assignee can access this method.
    /// <br />
    /// If there is no items in the requested page, then empty page will be returned.
    /// </remarks>
    /// <response code="200">Returns requested page with grades assigned in the group to the student.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role, not an owner of the group or not the grade's assignee</response>
    /// <response code="404">Group does not exist</response>
    [HttpGet]
    [Route("{groupId:int}/students/{studentId}/grades")]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(Page<StudentGroupViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGradesOfStudentInGroupAsync(
        [FromRoute] int groupId,
        [FromRoute] string studentId,
        [FromQuery] PaginationProperties properties,
        [FromQuery] AssignedGradesFilterProperties filterProperties)
    {
        string callerId = User.Identity.GetSubjectId();
        var response = await _groupsService.GetOwnerIdAsync(groupId);
        if (!response.GroupExists)
        {
            return NotFound(
                CustomResponses.NotFound("The group with the specified ID does not exist.",
                HttpContext));
        }
        // Forbid if user is not an administrator, and doesn't own the group,
        // and not the assignee of the grade
        if (response.OwnerId != callerId &&
            studentId != callerId &&
            !User.HasClaim(JwtClaimTypes.Role, Roles.Administrator))
        {
            return Forbid();
        }
        AssignedGradesFilter filter = new(filterProperties, studentId: studentId, groupId: groupId);
        return Ok(await _assignedGradesService
            .GetPageAsync<AssignedGradeViewDto>(properties, filter, includeGroups: false, includeStudents: false));
    }
}
