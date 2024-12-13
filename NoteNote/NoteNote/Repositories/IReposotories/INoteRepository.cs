using NoteNote.Dtos;
using NoteNote.Models;

namespace NoteNote.Repositories.IReposotories
{
    public interface INoteRepository
    {
        public Task<PaginatedResult<ViewNoteDto>> GetNotesAsync(int userId, int pageNumber, int pageSize, string searchQuery);
        public List<ViewNoteDto> GetNotesQueryable(int userId, string searchQuery);
        public Task<NoteDto> CreateNoteAsync(NoteDto note);
        public Task<ViewNoteDto> ViewNoteById(int noteId);
        public Task<updateNoteDto> UpdateNoteAsync(updateNoteDto updatedNote);
        public Task<bool> DeleteNoteAsync(int noteId);
        public Task<ViewNoteDto> ViewNewestNoteByUserId(int userId);
    }
}
