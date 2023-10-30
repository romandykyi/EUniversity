using EUniversity.Core.Dtos.University;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;
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

    public GroupsController(IGroupsService groupsService)
    {
        _groupsService = groupsService;
    }

    /// <summary>
    /// Gets a page with groups.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// </remarks>
    /// <response code="200">Returns requested page with groups.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    [HttpGet]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(Page<GroupViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGroupsPageAsync([FromQuery] PaginationProperties properties)
    {
        return Ok(await _groupsService.GetPageAsync(properties));
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateGroupAsync([FromBody] GroupCreateDto dto)
    {
        int id = await _groupsService.CreateAsync(dto);
        var routeValues = new { id };
        var body = new { id };
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
}
