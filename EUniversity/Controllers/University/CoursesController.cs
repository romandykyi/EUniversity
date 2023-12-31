﻿using EUniversity.Core.Dtos.University;
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
    /// If the query param 'semesterId' is 0, then courses that are not linked to any semesters will be returned.
    /// </remarks>
    /// <param name="properties">Pagination properties.</param>
    /// <param name="filterProperties">Pagination properties.</param>
    /// <param name="name">An optional name to filter courses by.</param>
    /// <param name="sortingMode">
    /// An optional sorting mode.
    /// <para>
    /// Possible values:
    /// </para>
    /// <ul>
    /// <li>default(or 0) - no sorting will be applied;</li>
    /// <li>name(or 1) - courses will be sorted by their name(from a to z), this mode is applied by default;</li>
    /// <li>nameDescending(or 2) - courses will be sorted by their name in descending order(from z to a);</li>
    /// <li>newest(or 3) - courses will be sorted by their creation date in descending order;</li>
    /// <li>oldest(or 4) - courses will be sorted by their creation date in ascending order.</li>
    /// </ul>
    /// </param>
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
        [FromQuery] CoursesFilterProperties filterProperties,
        [FromQuery] string? name,
        [FromQuery] DefaultFilterSortingMode sortingMode = DefaultFilterSortingMode.Name)
    {
        CoursesFilter filter = new(filterProperties, name ?? string.Empty, sortingMode);
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
