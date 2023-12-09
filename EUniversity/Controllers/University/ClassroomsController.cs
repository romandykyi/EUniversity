﻿using EUniversity.Core.Dtos.University;
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
/// Classrooms controller.
/// </summary>
[ApiController]
[Route("api/classrooms")]
[FluentValidationAutoValidation]
public class ClassroomsController : ControllerBase
{
    private readonly IClassroomsService _classroomsService;

    public ClassroomsController(IClassroomsService classroomsService)
    {
        _classroomsService = classroomsService;
    }

    /// <summary>
    /// Gets a page with classrooms.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// </remarks>
    /// <param name="properties">Pagination properties.</param>
    /// <param name="name">An optional name to filter classrooms by.</param>
    /// <param name="sortingMode">
    /// An optional sorting mode.
    /// <para>
    /// Possible values:
    /// </para>
    /// <ul>
    /// <li>default(or 0) - no sorting will be applied;</li>
    /// <li>name(or 1) - classrooms will be sorted by their name(from a to z), this mode is applied by default;</li>
    /// <li>nameDescending(or 2) - classrooms will be sorted by their name in descending order(from z to a);</li>
    /// <li>newest(or 3) - classrooms will be sorted by their creation date in descending order;</li>
    /// <li>oldest(or 4) - classrooms will be sorted by their creation date in ascending order.</li>
    /// </ul>
    /// </param>
    /// <response code="200">Returns requested page with classrooms.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    [HttpGet]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(Page<ClassroomViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetClassroomsPageAsync(
        [FromQuery] PaginationProperties properties,
        [FromQuery] string? name,
        [FromQuery] DefaultFilterSortingMode sortingMode = DefaultFilterSortingMode.Name)
    {
        DefaultFilter<Classroom> filter = new(name ?? string.Empty, sortingMode);
        return Ok(await _classroomsService.GetPageAsync(properties, filter));
    }

    /// <summary>
    /// Gets a classroom by its ID.
    /// </summary>
    /// <response code="200">Returns requested classroom</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="404">Classroom does not exist</response>
    [HttpGet("{id:int}", Name = nameof(GetClassroomByIdAsync))]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(ClassroomViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClassroomByIdAsync([FromRoute] int id)
    {
        var classroom = await _classroomsService.GetByIdAsync(id);
        return classroom != null ? Ok(classroom) : NotFound();
    }

    /// <summary>
    /// Creates a new classroom.
    /// </summary>
    /// <response code="201">Successfully created</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpPost]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(typeof(ClassroomViewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateClassroomAsync([FromBody] ClassroomCreateDto dto)
    {
        var classroom = await _classroomsService.CreateAsync(dto);
        var routeValues = new { id = classroom.Id };
        var body = classroom.Adapt<ClassroomViewDto>();
        return CreatedAtRoute(nameof(GetClassroomByIdAsync), routeValues, body);
    }

    /// <summary>
    /// Edits a classroom by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Classroom does not exist</response>
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateClassroomAsync([FromRoute] int id,
        [FromBody] ClassroomCreateDto dto)
    {
        var result = await _classroomsService.UpdateAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes a classroom by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Classroom does not exist</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteClassroomAsync([FromRoute] int id)
    {
        var result = await _classroomsService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
