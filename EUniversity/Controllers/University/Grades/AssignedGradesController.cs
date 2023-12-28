using Duende.IdentityServer.Extensions;
using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;
using EUniversity.Core.Services.University.Grades;
using EUniversity.Infrastructure.Filters;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace EUniversity.Controllers.University;

/// <summary>
/// Assigned grades controller.
/// </summary>
[ApiController]
[Route("api/assignedGrades")]
[FluentValidationAutoValidation]
public class AssignedGradesController : ControllerBase
{
    private readonly IGroupsService _groupsService;
    private readonly IAssignedGradesService _assignedAssignedGradesService;

    public AssignedGradesController(IGroupsService groupsService, IAssignedGradesService assignedAssignedGradesService)
    {
        _groupsService = groupsService;
        _assignedAssignedGradesService = assignedAssignedGradesService;
    }

    /// <summary>
    /// Gets a page with assigned grades.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// </remarks>
    /// <param name="properties">Pagination properties.</param>
    /// <param name="filterProperties"></param>
    /// <response code="200">Returns requested page with grades.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpGet]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(typeof(Page<AssignedGradeViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAssignedGradesPageAsync(
        [FromQuery] PaginationProperties properties,
        [FromQuery] AssignedGradesFilterProperties filterProperties)
    {
        AssignedGradesFilter filter = new(filterProperties);
        return Ok(await _assignedAssignedGradesService
            .GetPageAsync<AssignedGradeViewDto>(properties, filter));
    }

    /// <summary>
    /// Creates a new assigned grade.
    /// </summary>
    /// <remarks>
    /// Only administrators and group owners can use this method.
    /// </remarks>
    /// <response code="201">Successfully created</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role or tries to add a grade to the group which it doesn't own</response>
    [HttpPost]
    [Authorize(Policies.IsTeacherOrAdministrator)]
    [ProducesResponseType(typeof(GradeViewDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateAssignedGradeAsync([FromBody] AssignedGradeCreateDto dto)
    {
        string callerId = User.Identity.GetSubjectId();
        // If user is not an administrator then check if it is owner
        if (!User.HasClaim(JwtClaimTypes.Role, Roles.Administrator))
        {
            var response = await _groupsService.GetOwnerIdAsync(dto.GroupId);
            if (callerId != response.OwnerId)
            {
                return Forbid();
            }
        }

        await _assignedAssignedGradesService.AssignAsync(dto, callerId);
        return NoContent();
    }

    /// <summary>
    /// Edits an assigned grade by its ID.
    /// </summary>
    /// <remarks>
    /// Only administrators and users that assigned the grade can use this method.
    /// </remarks>
    /// <response code="204">Success</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role or is not the assigner of the grade</response>
    /// <response code="404">Grade does not exist</response>
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policies.IsTeacherOrAdministrator)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAssignedGradeAsync([FromRoute] int id,
        [FromBody] AssignedGradeUpdateDto dto)
    {
        string callerId = User.Identity.GetSubjectId();
        // If user is not an administrator then check if it is the assigner
        if (!User.HasClaim(JwtClaimTypes.Role, Roles.Administrator))
        {
            var response = await _assignedAssignedGradesService.GetAssignerIdAsync(id);
            // We can detect the 404 error early here
            if (!response.GradeExists)
            {
                return NotFound();
            }
            if (callerId != response.AssignerId)
            {
                return Forbid();
            }
        }
        var result = await _assignedAssignedGradesService.ReassignAsync(id, dto, callerId);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes an assigned grade by its ID.
    /// </summary>
    /// <remarks>
    /// Only administrators and users that assigned the grade can use this method.
    /// </remarks>
    /// <response code="204">Success</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role or is not the assigner of the grade</response>
    /// <response code="404">Grade does not exist</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policies.IsTeacherOrAdministrator)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGradeAsync([FromRoute] int id)
    {
        string callerId = User.Identity.GetSubjectId();
        // If user is not an administrator then check if it is the assigner
        if (!User.HasClaim(JwtClaimTypes.Role, Roles.Administrator))
        {
            var response = await _assignedAssignedGradesService.GetAssignerIdAsync(id);
            // We can detect the 404 error early here
            if (!response.GradeExists)
            {
                return NotFound();
            }
            if (callerId != response.AssignerId)
            {
                return Forbid();
            }
        }
        var result = await _assignedAssignedGradesService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
