using EUniversity.Core.Dtos.University;
using EUniversity.Core.Policy;
using EUniversity.Infrastructure.Services.University;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace EUniversity.Controllers.University
{
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
        /// Gets a classroom by its ID.
        /// </summary>
        /// <response code="200">Returns requested classroom</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Classroom does not exist</response>
        [HttpGet("{id:int}", Name = nameof(GetByIdAsync))]
        [Authorize(Policies.Default)]
        [ProducesResponseType(typeof(ViewClassroomDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var classroom = await _classroomsService.GetByIdAsync(id);
            return classroom != null ? Ok(classroom) : NotFound();
        }

        /// <summary>
        /// Creates a new classroom by its ID.
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
        public async Task<IActionResult> CreateAsync([FromBody] CreateClassromDto dto)
        {
            int id = await _classroomsService.CreateAsync(dto);
            var routeValues = new { id };
            var body = new { id };
            return CreatedAtRoute(nameof(GetByIdAsync), routeValues, body);
        }

        /// <summary>
        /// Edits a classroom.
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
        public async Task<IActionResult> UpdateAsync([FromRoute] int id,
            [FromBody] CreateClassromDto dto)
        {
            var result = await _classroomsService.UpdateAsync(id, dto);
            return result ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes a classroom.
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
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var result = await _classroomsService.DeleteAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
