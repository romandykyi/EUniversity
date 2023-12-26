using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using EUniversity.Core.Services.University;
using EUniversity.Infrastructure.Filters;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace EUniversity.Controllers.University;

/// <summary>
/// Semesters controller.
/// </summary>
[ApiController]
[Route("api/semesters")]
[FluentValidationAutoValidation]
public class SemestersController : ControllerBase
{
    private readonly ISemestersService _semestersService;
    private readonly IStudentSemestersService _studentSemestersService;
    private readonly IEntityExistenceChecker _existenceChecker;

    public SemestersController(ISemestersService semestersService,
        IStudentSemestersService studentSemestersService,
        IEntityExistenceChecker existenceChecker)
    {
        _semestersService = semestersService;
        _studentSemestersService = studentSemestersService;
        _existenceChecker = existenceChecker;
    }

    /// <summary>
    /// Gets a page with semesters.
    /// </summary>
    /// <remarks>
    /// If there is no items in the requested page, then empty page will be returned.
    /// </remarks>
    /// <param name="properties">Pagination properties.</param>
    /// <param name="filterProperties">Semesters filter properties.</param>
    /// <param name="name">An optional name to filter semesters by.</param>
    /// <param name="sortingMode">
    /// An optional sorting mode.
    /// <para>
    /// Possible values:
    /// </para>
    /// <ul>
    /// <li>default(or 0) - no sorting will be applied;</li>
    /// <li>name(or 1) - semesters will be sorted by their name(from a to z), this mode is applied by default;</li>
    /// <li>nameDescending(or 2) - semesters will be sorted by their name in descending order(from z to a);</li>
    /// <li>newest(or 3) - semesters will be sorted by their creation date in descending order;</li>
    /// <li>oldest(or 4) - semesters will be sorted by their creation date in ascending order.</li>
    /// </ul>
    /// </param>
    /// <response code="200">Returns requested page with semesters.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    [HttpGet]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(Page<SemesterViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetSemestersPageAsync(
        [FromQuery] PaginationProperties properties,
        [FromQuery] SemestersFilterProperties filterProperties,
        [FromQuery] string? name,
        [FromQuery] DefaultFilterSortingMode sortingMode = DefaultFilterSortingMode.Name)
    {
        SemestersFilter filter = new(filterProperties, name ?? string.Empty, sortingMode);
        return Ok(await _semestersService.GetPageAsync(properties, filter));
    }

    /// <summary>
    /// Gets a semester by its ID.
    /// </summary>
    /// <response code="200">Returns requested semester</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="404">Semester does not exist</response>
    [HttpGet("{id:int}", Name = nameof(GetSemesterByIdAsync))]
    [Authorize(Policies.Default)]
    [ProducesResponseType(typeof(SemesterViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSemesterByIdAsync([FromRoute] int id)
    {
        var semester = await _semestersService.GetByIdAsync(id);
        return semester != null ? Ok(semester) : NotFound();
    }

    /// <summary>
    /// Creates a new semester.
    /// </summary>
    /// <response code="201">Successfully created</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    [HttpPost]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(typeof(SemesterViewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateSemesterAsync([FromBody] SemesterCreateDto dto)
    {
        var semester = await _semestersService.CreateAsync(dto);
        var routeValues = new { id = semester.Id };
        var body = semester.Adapt<SemesterViewDto>();
        return CreatedAtRoute(nameof(GetSemesterByIdAsync), routeValues, body);
    }

    /// <summary>
    /// Edits a semester by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Semester does not exist</response>
    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSemesterAsync([FromRoute] int id,
        [FromBody] SemesterCreateDto dto)
    {
        var result = await _semestersService.UpdateAsync(id, dto);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes a semester by its ID.
    /// </summary>
    /// <response code="204">Success</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Semester does not exist</response>
    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSemesterAsync([FromRoute] int id)
    {
        var result = await _semestersService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }

    /// <summary>
    /// Gets a page with all students related to the semester with the given ID.
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
    /// <response code="200">Returns requested page with students related to the semester.</response>
    /// <response code="400">Bad request</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">Caller is not a teacher or an administrator</response>
    [HttpGet]
    [Route("{semesterId:int}/students")]
    [Authorize(Policies.IsTeacherOrAdministrator)]
    [ProducesResponseType(typeof(Page<StudentSemesterViewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentsInSemesterAsync(
        [FromRoute] int semesterId,
        [FromQuery] PaginationProperties properties,
        [FromQuery] StudentEnrollmentsFilterProperties filterProperties)
    {
        if (!await _existenceChecker.ExistsAsync<Semester, int>(semesterId))
        {
            return NotFound(
                CustomResponses.NotFound("The semester with the specified ID does not exist.",
                HttpContext));
        }
        StudentEnrollmentsFilter<StudentSemester> filter = new(filterProperties);
        return Ok(await _studentSemestersService
            .GetAssigningEntitiesPageAsync<StudentSemesterViewDto>(semesterId, properties, filter));
    }

    /// <summary>
    /// Adds a student to a semester.
    /// </summary>
    /// <response code="200">Success(user is already part of the semester)</response>
    /// <response code="201">Success(user was added to the semester)</response>
    /// <response code="400">Malformed input, or invalid user ID, or user is not a student</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Semester does not exist</response>
    [HttpPost]
    [Route("{semesterId:int}/students")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddStudentToSemesterAsync(
        [FromRoute] int semesterId, [FromBody] AssignStudentDto dto)
    {
        if (!await _existenceChecker.ExistsAsync<Semester, int>(semesterId))
        {
            return NotFound(
                CustomResponses.NotFound("The semester with the specified ID does not exist.",
                HttpContext));
        }
        bool created = await _studentSemestersService.AssignAsync(semesterId, dto.StudentId);

        if (created)
        {
            var routeValues = new { semesterId, studentId = dto.StudentId };
            return CreatedAtRoute(nameof(DeleteStudentFromSemesterAsync), routeValues, null);
        }
        return Ok(new { message = "The student is already part of the semester" });

    }

    /// <summary>
    /// Deletes a student from a semester.
    /// </summary>
    /// <response code="204">Success(user was removed from the semester)</response>
    /// <response code="400">Malformed/invalid input</response>
    /// <response code="401">Unauthorized user call</response>
    /// <response code="403">User lacks 'Administrator' role</response>
    /// <response code="404">Semester/Student does not exist or student is not part of the semester</response>
    [HttpDelete]
    [Route("{semesterId:int}/students/{studentId}")]
    [Authorize(Policies.HasAdministratorPermission)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStudentFromSemesterAsync(
        [FromRoute] int semesterId, [FromRoute] string studentId)
    {
        if (!await _existenceChecker.ExistsAsync<Semester, int>(semesterId))
        {
            return NotFound(
                CustomResponses.NotFound("The semester with the specified ID does not exist.",
                HttpContext));
        }
        if (!await _existenceChecker.ExistsAsync<ApplicationUser, string>(studentId))
        {
            return NotFound(
                CustomResponses.NotFound("The user with the specified ID does not exist.",
                HttpContext));
        }

        bool removed = await _studentSemestersService.UnassignAsync(semesterId, studentId);
        if (removed)
        {
            return NoContent();
        }
        return NotFound(
                CustomResponses.NotFound("The user is not part of the semester.",
                HttpContext));
    }
}
