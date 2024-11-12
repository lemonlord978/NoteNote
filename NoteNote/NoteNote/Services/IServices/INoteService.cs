using NoteNote.Dtos;
using NoteNote.Models;

namespace NoteNote.Services.IServices
{
    public interface INoteService
    {
        public Task<PaginatedResult<ViewNoteDto>> GetNotesAsync(int userId, int pageNumber, int pageSize, string searchQuery);
        public Task<NoteDto> CreateNoteAsync(NoteDto note);
        public Task<NoteViewDto> ViewNoteById(int noteId);
        public Task<updateNoteDto> UpdateNoteAsync(updateNoteDto updatedNote);
        public Task<bool> DeleteNoteAsync(int noteId);
        public Task<NoteViewDto> ViewNewestNoteByUserId(int userId);
    }
}
