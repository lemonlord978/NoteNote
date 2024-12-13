using Microsoft.EntityFrameworkCore;
using NoteNote.Dtos;
using NoteNote.Models;
using NoteNote.Repositories.IReposotories;
using NoteNote.Services.IServices;

namespace NoteNote.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }
        public async Task<PaginatedResult<ViewNoteDto>> GetNotesAsync(int userId, int pageNumber, int pageSize, string searchQuery)
        {
            var data = await _noteRepository.GetNotesAsync(userId, pageNumber, pageSize, searchQuery);
            return data;
        }
        public async Task<NoteDto> CreateNoteAsync(NoteDto note)
        {
            await _noteRepository.CreateNoteAsync(note);
            return note;
        }

        public async Task<ViewNoteDto> ViewNoteById(int noteId)
        {
            var data =  await _noteRepository.ViewNoteById(noteId);
            return data;
        }

        public async Task<updateNoteDto> UpdateNoteAsync(updateNoteDto updatedNote)
        {
            var data = await _noteRepository.UpdateNoteAsync(updatedNote);
            return data;
        }

        public async Task<bool> DeleteNoteAsync(int noteId)
        {
            var data = await _noteRepository.DeleteNoteAsync(noteId);
            return data;
        }

        public async Task<ViewNoteDto> ViewNewestNoteByUserId(int userId)
        {
            var data = await _noteRepository.ViewNewestNoteByUserId(userId);
            return data;
        }

        public List<ViewNoteDto> GetNotesQueryable(int userId, string searchQuery)
        {
            var data = _noteRepository.GetNotesQueryable(userId, searchQuery);
            return data;
        }
    }
}
