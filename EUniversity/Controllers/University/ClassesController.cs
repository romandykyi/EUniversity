using EUniversity.Core.Dtos.University;
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
/// Classes controller.
/// </summary>
[ApiController]
[Route("api/classes")]
[FluentValidationAutoValidation]
public class ClassesController : ControllerBase
{
    private readonly IClassesService _classesService;

    public ClassesController(IClassesService classesService)
    {
        _classesService = classesService;
    }

    /// <summary>
    /// Gets a page with classes.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// </remarks>
    /// <param name="properties">Pagination properties.</param>
    /// <param name="filterProperties">Optional filter properties.</param>
    /// <response code="200">Returns requested page with classes.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    [HttpGet]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(Page<ClassViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetClassesPageAsync(
        [FromQuery] PaginationProperties properties,
        [FromQuery] ClassesFilterProperties filterProperties)
    {
        ClassesFilter filter = new(filterProperties);
        return Ok(await _classesService.GetPageAsync(properties, filter));
    }

    /// <summary>
    /// Gets a class by its ID.
    /// </summary>
    /// <response code="200">Returns requested class</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="404">Class does not exist</response>
    [HttpGet("{id:int}", Name = nameof(GetClassByIdAsync))]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(ClassViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClassByIdAsync([FromRoute] int id)
    {
        var @class = await _classesService.GetByIdAsync(id);
        return @class != null ? Ok(@class) : NotFound();
    }

    /// <summary>
    /// Creates a new class.
    /// </summary>
    /// <response code="201">Successfully created</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpPost]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(typeof(ClassViewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateClassAsync([FromBody] ClassCreateDto dto)
    {
        var @class = await _classesService.CreateAsync(dto);
        var routeValues = new { id = @class.Id };
        var body = @class.Adapt<ClassViewDto>();
        return CreatedAtRoute(nameof(GetClassByIdAsync), routeValues, body);
    }

    /// <summary>
    /// Edits a class by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Class does not exist</response>
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateClassAsync([FromRoute] int id,
        [FromBody] ClassUpdateDto dto)
    {
        var result = await _classesService.UpdateAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes a class by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Class does not exist</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteClassAsync([FromRoute] int id)
    {
        var result = await _classesService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
