﻿using EUniversity.Core.Dtos.University.Grades;
using EUniversity.Core.Pagination;
using EUniversity.Core.Policy;
using EUniversity.Infrastructure.Services.University;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace EUniversity.Controllers.University
{
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
        /// <response code="200">Returns requested page with grades.</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized user call</response>
        [HttpGet]
        [Authorize(Policies.Default)]
        [ProducesResponseType(typeof(Page<GradeCreateDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetGradesPageAsync([FromQuery] PaginationProperties properties)
        {
            return Ok(await _gradesService.GetPageAsync(properties));
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
        /// Creates a new grade by its ID.
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
        public async Task<IActionResult> CreateGradeAsync([FromBody] GradeCreateDto dto)
        {
            int id = await _gradesService.CreateAsync(dto);
            var routeValues = new { id };
            var body = new { id };
            return CreatedAtRoute(nameof(GetGradeByIdAsync), routeValues, body);
        }

        /// <summary>
        /// Edits a grade.
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
        /// Deletes a grade.
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
}
