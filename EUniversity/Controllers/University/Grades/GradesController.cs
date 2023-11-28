﻿using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Infrastructure.Filters;
using EUniversity.Infrastructure.Services.University;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace EUniversity.Controllers.University;

/// <summary>
/// Grades controller.
/// </summary>
[ApiController]
[Route("api/grades")]
[FluentValidationAutoValidation]
public class GradesController : ControllerBase
{
    private readonly IGradesService _gradesService;

    public GradesController(IGradesService gradesService)
    {
        _gradesService = gradesService;
    }

    /// <summary>
    /// Gets a page with grades.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// </remarks>
    /// <param name="properties">Pagination properties.</param>
    /// <param name="name">An optional name to filter grades by.</param>
    /// <param name="sortingMode">
    /// An optional sorting mode.
    /// <para>
    /// Possible values:
    /// </para>
    /// <ul>
    /// <li>default(or 0) - no sorting will be applied;</li>
    /// <li>name(or 1) - grades will be sorted by their name(from a to z), this mode is applied by default;</li>
    /// <li>nameDescending(or 2) - grades will be sorted by their name in descending order(from z to a);</li>
    /// <li>newest(or 3) - grades will be sorted by their creation date in descending order;</li>
    /// <li>oldest(or 4) - grades will be sorted by their creation date in ascending order.</li>
    /// </ul>
    /// </param>
    /// <response code="200">Returns requested page with grades.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    [HttpGet]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(Page<GradeCreateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGradesPageAsync(
        [FromQuery] PaginationProperties properties,
        [FromQuery] string? name,
        [FromQuery] DefaultFilterSortingMode sortingMode = DefaultFilterSortingMode.Name)
    {
        DefaultFilter<Grade> filter = new(name ?? string.Empty, sortingMode);
        return Ok(await _gradesService.GetPageAsync(properties, filter));
    }

    /// <summary>
    /// Gets a grade by its ID.
    /// </summary>
    /// <response code="200">Returns requested grade</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="404">Grade does not exist</response>
    [HttpGet("{id:int}", Name = nameof(GetGradeByIdAsync))]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(GradeCreateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGradeByIdAsync([FromRoute] int id)
    {
        var grade = await _gradesService.GetByIdAsync(id);
        return grade != null ? Ok(grade) : NotFound();
    }

    /// <summary>
    /// Creates a new grade.
    /// </summary>
    /// <response code="201">Successfully created</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpPost]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(typeof(GradeViewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateGradeAsync([FromBody] GradeCreateDto dto)
    {
        var grade = await _gradesService.CreateAsync(dto);
        var routeValues = new { id = grade.Id };
        var body = grade.Adapt<GradeViewDto>();
        return CreatedAtRoute(nameof(GetGradeByIdAsync), routeValues, body);
    }

    /// <summary>
    /// Edits a grade by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Grade does not exist</response>
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGradeAsync([FromRoute] int id,
        [FromBody] GradeCreateDto dto)
    {
        var result = await _gradesService.UpdateAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes a grade by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Grade does not exist</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteGradeAsync([FromRoute] int id)
    {
        var result = await _gradesService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
