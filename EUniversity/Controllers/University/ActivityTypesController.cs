using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Filters;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace EUniversity.Controllers.University;

/// <summary>
/// Activity types controller.
/// </summary>
[ApiController]
[Route("api/activityTypes")]
[FluentValidationAutoValidation]
public class ActivityTypesController : ControllerBase
{
    private readonly IActivityTypesService _activityTypesService;

    public ActivityTypesController(IActivityTypesService activityTypesService)
    {
        _activityTypesService = activityTypesService;
    }

    /// <summary>
    /// Gets a page with activity types.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// </remarks>
    /// <param name="properties">Pagination properties.</param>
    /// <param name="name">An optional name to filter activityTypes by.</param>
    /// <param name="sortingMode">
    /// An optional sorting mode.
    /// <para>
    /// Possible values:
    /// </para>
    /// <ul>
    /// <li>default(or 0) - no sorting will be applied;</li>
    /// <li>name(or 1) - activity types will be sorted by their name(from a to z), this mode is applied by default;</li>
    /// <li>nameDescending(or 2) - activity types will be sorted by their name in descending order(from z to a);</li>
    /// <li>newest(or 3) - activity types will be sorted by their creation date in descending order;</li>
    /// <li>oldest(or 4) - activity types will be sorted by their creation date in ascending order.</li>
    /// </ul>
    /// </param>
    /// <response code="200">Returns requested page with activityTypes.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    [HttpGet]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(Page<ActivityTypeViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetActivityTypesPageAsync(
        [FromQuery] PaginationProperties properties,
        [FromQuery] string? name,
        [FromQuery] DefaultFilterSortingMode sortingMode = DefaultFilterSortingMode.Name)
    {
        DefaultFilter<ActivityType> filter = new(name ?? string.Empty, sortingMode);
        return Ok(await _activityTypesService.GetPageAsync(properties, filter));
    }

    /// <summary>
    /// Gets an activity type by its ID.
    /// </summary>
    /// <response code="200">Returns requested activityType</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="404">ActivityType does not exist</response>
    [HttpGet("{id:int}", Name = nameof(GetActivityTypeByIdAsync))]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(ActivityTypeViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActivityTypeByIdAsync([FromRoute] int id)
    {
        var activityType = await _activityTypesService.GetByIdAsync(id);
        return activityType != null ? Ok(activityType) : NotFound();
    }

    /// <summary>
    /// Creates a new activity type.
    /// </summary>
    /// <response code="201">Successfully created</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpPost]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(typeof(ActivityTypeViewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateActivityTypeAsync([FromBody] ActivityTypeCreateDto dto)
    {
        var activityType = await _activityTypesService.CreateAsync(dto);
        var routeValues = new { id = activityType.Id };
        var body = activityType.Adapt<ActivityTypeViewDto>();
        return CreatedAtRoute(nameof(GetActivityTypeByIdAsync), routeValues, body);
    }

    /// <summary>
    /// Edits an activity type by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">ActivityType does not exist</response>
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateActivityTypeAsync([FromRoute] int id,
        [FromBody] ActivityTypeCreateDto dto)
    {
        var result = await _activityTypesService.UpdateAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes an activity type by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">ActivityType does not exist</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteActivityTypeAsync([FromRoute] int id)
    {
        var result = await _activityTypesService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
