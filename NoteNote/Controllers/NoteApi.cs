using Microsoft.AspNetCore.Mvc;
using NoteNote.Dtos;
using NoteNote.Models;
using NoteNote.Services.IServices;

namespace NoteNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteApi : ControllerBase
    {
        private readonly INoteService _noteService;
        public NoteApi(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<PaginatedResult<ViewNoteDto>>> GetAllNote(
        [FromRoute] int userId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string searchQuery = "")
        {
            try
            {
                // Call the service to get the paginated results with search query applied
                var paginatedNotes = await _noteService.GetNotesAsync(userId, pageNumber, pageSize, searchQuery);

                if (paginatedNotes.Items.Any())
                {
                    // Return the paginated result
                    return Ok(paginatedNotes);
                }
                else
                {
                    return NotFound("No notes found for the specified user.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("getNoteById/{noteId}")]
        public async Task<ActionResult<NoteViewDto>> GetNoteById([FromRoute] int noteId)
        {
            try
            {
                var data = await _noteService.ViewNoteById(noteId);
                if (data != null)
                {
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getNewestNote/{userId}")]
        public async Task<ActionResult<NoteViewDto>> getNewestNote([FromRoute] int userId)
        {
            try
            {
                var data = await _noteService.ViewNewestNoteByUserId(userId);
                if (data != null)
                {
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<NoteApi>
        [HttpPost("createNote")]
        public async Task<ActionResult> CreateNote([FromBody] NoteDto note)
        {
            try
            {
                var data = await _noteService.CreateNoteAsync(note);
                return Ok(new { message = "Created successfully"});
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("updateNote")]
        public async Task<IActionResult> UpdateNoteAsync([FromBody] updateNoteDto noteDto)
        {
            try
            {
                var updatedNote = await _noteService.UpdateNoteAsync(new updateNoteDto
                {
                    NoteId = noteDto.NoteId,
                    Title = noteDto.Title,
                    Content = noteDto.Content,
                    Tags = noteDto.Tags
                });

                return Ok(updatedNote);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Note not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("deleteNote/{noteId}")]
        public async Task<IActionResult> DeleteNoteAsync(int noteId)
        {
            try
            {
                bool result = await _noteService.DeleteNoteAsync(noteId);

                if (result)
                {
                    return Ok(new { message = "Note deleted successfully." });
                }
                else
                {
                    return NotFound(new { message = "Note not found." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
