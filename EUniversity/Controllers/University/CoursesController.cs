using Bogus.DataSets;
using EUniversity.Core.Dtos.University;
using EUniversity.Core.Filters;
using EUniversity.Core.Models.University;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.University;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace EUniversity.Controllers.University;

/// <summary>
/// Courses controller.
/// </summary>
[ApiController]
[Route("api/courses")]
[FluentValidationAutoValidation]
public class CoursesController : ControllerBase
{
    private readonly ICoursesService _coursesService;

    public CoursesController(ICoursesService coursesService)
    {
        _coursesService = coursesService;
    }

    /// <summary>
    /// Gets a page with courses.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// </remarks>
    /// <param name="properties">Pagination properties.</param>
    /// <param name="name">An optional name to filter courses by.</param>
    /// <response code="200">Returns requested page with courses.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    [HttpGet]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(Page<CourseViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCoursesPageAsync(
        [FromQuery] PaginationProperties properties,
        [FromQuery] string? name)
    {
        NameFilter<Course>? filter = name != null ? new(name) : null;
        return Ok(await _coursesService.GetPageAsync(properties, filter));
    }

    /// <summary>
    /// Gets a course by its ID.
    /// </summary>
    /// <response code="200">Returns requested course</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="404">Course does not exist</response>
    [HttpGet("{id:int}", Name = nameof(GetCourseByIdAsync))]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(CourseViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseByIdAsync([FromRoute] int id)
    {
        var course = await _coursesService.GetByIdAsync(id);
        return course != null ? Ok(course) : NotFound();
    }

    /// <summary>
    /// Creates a new course.
    /// </summary>
    /// <response code="201">Successfully created</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpPost]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(typeof(CourseViewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateCourseAsync([FromBody] CourseCreateDto dto)
    {
        var course = await _coursesService.CreateAsync(dto);
        var routeValues = new { id = course.Id };
        var body = course.Adapt<CourseViewDto>();
        return CreatedAtRoute(nameof(GetCourseByIdAsync), routeValues, body);
    }

    /// <summary>
    /// Edits a course by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Course does not exist</response>
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCourseAsync([FromRoute] int id,
        [FromBody] CourseCreateDto dto)
    {
        var result = await _coursesService.UpdateAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes a course by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Course does not exist</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCourseAsync([FromRoute] int id)
    {
        var result = await _coursesService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
