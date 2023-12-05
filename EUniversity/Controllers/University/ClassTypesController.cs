using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models.University;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Filters;
using EUniversity.Infrastructure.Services.University;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace EUniversity.Controllers.University;

/// <summary>
/// ClassTypes controller.
/// </summary>
[ApiController]
[Route("api/classTypes")]
[FluentValidationAutoValidation]
public class ClassTypesController : ControllerBase
{
    private readonly IClassTypesService _classTypesService;

    public ClassTypesController(IClassTypesService classTypesService)
    {
        _classTypesService = classTypesService;
    }

    /// <summary>
    /// Gets a page with classTypes.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// </remarks>
    /// <param name="properties">Pagination properties.</param>
    /// <param name="name">An optional name to filter classTypes by.</param>
    /// <param name="sortingMode">
    /// An optional sorting mode.
    /// <para>
    /// Possible values:
    /// </para>
    /// <ul>
    /// <li>default(or 0) - no sorting will be applied;</li>
    /// <li>name(or 1) - classTypes will be sorted by their name(from a to z), this mode is applied by default;</li>
    /// <li>nameDescending(or 2) - classTypes will be sorted by their name in descending order(from z to a);</li>
    /// <li>newest(or 3) - classTypes will be sorted by their creation date in descending order;</li>
    /// <li>oldest(or 4) - classTypes will be sorted by their creation date in ascending order.</li>
    /// </ul>
    /// </param>
    /// <response code="200">Returns requested page with classTypes.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    [HttpGet]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(Page<ClassTypeViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetClassTypesPageAsync(
        [FromQuery] PaginationProperties properties,
        [FromQuery] string? name,
        [FromQuery] DefaultFilterSortingMode sortingMode = DefaultFilterSortingMode.Name)
    {
        DefaultFilter<ClassType> filter = new(name ?? string.Empty, sortingMode);
        return Ok(await _classTypesService.GetPageAsync(properties, filter));
    }

    /// <summary>
    /// Gets a classType by its ID.
    /// </summary>
    /// <response code="200">Returns requested classType</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="404">ClassType does not exist</response>
    [HttpGet("{id:int}", Name = nameof(GetClassTypeByIdAsync))]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(ClassTypeViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClassTypeByIdAsync([FromRoute] int id)
    {
        var classType = await _classTypesService.GetByIdAsync(id);
        return classType != null ? Ok(classType) : NotFound();
    }

    /// <summary>
    /// Creates a new classType.
    /// </summary>
    /// <response code="201">Successfully created</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpPost]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(typeof(ClassTypeViewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateClassTypeAsync([FromBody] ClassTypeCreateDto dto)
    {
        var classType = await _classTypesService.CreateAsync(dto);
        var routeValues = new { id = classType.Id };
        var body = classType.Adapt<ClassTypeViewDto>();
        return CreatedAtRoute(nameof(GetClassTypeByIdAsync), routeValues, body);
    }

    /// <summary>
    /// Edits a classType by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">ClassType does not exist</response>
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateClassTypeAsync([FromRoute] int id,
        [FromBody] ClassTypeCreateDto dto)
    {
        var result = await _classTypesService.UpdateAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes a classType by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">ClassType does not exist</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteClassTypeAsync([FromRoute] int id)
    {
        var result = await _classTypesService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
