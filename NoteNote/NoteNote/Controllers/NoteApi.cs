using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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

        //[EnableQuery]
        //[HttpGet("{userId}")]
        //public async Task<ActionResult<PaginatedResult<ViewNoteDto>>> GetAllNote(
        //    [FromRoute] int userId,
        //    [FromQuery] int pageNumber = 1,
        //    [FromQuery] int pageSize = 10,
        //    [FromQuery] string searchQuery = "")
        //{
        //    try
        //    {
        //        // Lấy IQueryable từ service
        //        var notesQuery = _noteService.GetNotesQueryable(userId, searchQuery);

        //        // Áp dụng các truy vấn OData trước khi phân trang
        //        var filteredNotes = notesQuery.ToList(); // Thực hiện sau khi áp dụng OData
        //        var paginatedNotes = new PaginatedResult<ViewNoteDto>
        //        {
        //            Items = filteredNotes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
        //            TotalCount = filteredNotes.co
        //        };

        //        return Ok(paginatedNotes);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"An error occurred: {ex.Message}");
        //    }
        //}
        [Authorize]
        [EnableQuery]
        [HttpGet]
        public IActionResult GetNotes([FromQuery] int userId, [FromQuery] string searchQuery = "")
        {
            try
            {
                var notes = _noteService.GetNotesQueryable(userId, searchQuery);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getNoteById/{noteId}")]
        public async Task<ActionResult<ViewNoteDto>> GetNoteById([FromRoute] int noteId)
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

        [HttpGet("shareNote/{noteId}")]
        public async Task<ActionResult<ViewNoteDto>> ShareNote([FromRoute] int noteId)
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

        [Authorize]
        [HttpGet("getNewestNote/{userId}")]
        public async Task<ActionResult<ViewNoteDto>> getNewestNote([FromRoute] int userId)
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

        [Authorize]
        [HttpPost("createNote")]
        public async Task<ActionResult> CreateNote([FromBody] NoteDto note)
        {
            try
            {
                var data = await _noteService.CreateNoteAsync(note);
                return Ok(new { message = "Created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("updateNote")]
        public async Task<IActionResult> UpdateNoteAsync([FromBody] updateNoteDto noteDto)
        {
            try
            {
                var updatedNote = await _noteService.UpdateNoteAsync(noteDto);

                return Ok(noteDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Note not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
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
