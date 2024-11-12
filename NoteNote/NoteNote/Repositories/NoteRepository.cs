using Microsoft.EntityFrameworkCore;
using NoteNote.DBContext;
using NoteNote.Dtos;
using NoteNote.Models;
using NoteNote.Repositories.IReposotories;

namespace NoteNote.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly NoteAppContext _context;
        public NoteRepository(NoteAppContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<ViewNoteDto>> GetNotesAsync(int userId, int pageNumber, int pageSize, string searchQuery)
        {
            var query = _context.Notes.AsQueryable();

            query = query.Where(p => p.UserId == userId);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(note =>
                    note.Title.Contains(searchQuery) ||
                    note.NoteTags.Any(nt => nt.Tag.Name.Contains(searchQuery))
                );
            }

            var totalCount = await query.CountAsync();

            var notes = await query
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize)
                .Select(note => new ViewNoteDto
                {
                    NoteId = note.NoteId,
                    Title = note.Title,
                    Content = note.Content,
                    UserId = note.UserId,
                    UpdatedAt = note.UpdatedAt,
                    Tags = note.NoteTags.Select(nt => nt.Tag.Name).ToList()
                })
                .ToListAsync();

            return new PaginatedResult<ViewNoteDto>(notes, totalCount, pageNumber, pageSize);
        }

        public async Task<NoteDto> CreateNoteAsync(NoteDto note)
        {
            var data = new Note
            {
                UserId = note.UserId,
                Title = note.Title,
                Content = note.Content,
            };
            await _context.Notes.AddAsync(data);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<NoteViewDto> ViewNoteById(int noteId)
        {
            var note = await _context.Notes
                                     .Include(p => p.NoteTags)
                                         .ThenInclude(nt => nt.Tag)
                                     .FirstOrDefaultAsync(p => p.NoteId == noteId);

            if (note == null)
            {
                return null;
            }

            var data = new NoteViewDto
            {
                NoteId = note.NoteId,
                Title = note.Title,
                Content = note.Content,
                UserId = note.UserId,
                Tags = note.NoteTags.Select(nt => nt.Tag.Name).ToList()
            };

            return data;
        }

        public async Task<NoteViewDto> ViewNewestNoteByUserId(int userId)
        {
            var note = await _context.Notes
                                 .Where(p => p.UserId == userId)
                                 .OrderByDescending(p => p.CreatedAt)
                                 .FirstOrDefaultAsync();
            var data = new NoteViewDto
            {
                NoteId = note.NoteId,
                Title = note.Title,
                Content = note.Content,
                UserId = note.UserId,
            };
            return data;
        }

        public async Task<updateNoteDto> UpdateNoteAsync(updateNoteDto updatedNote)
        {
            var existingNote = await _context.Notes
                                              .Include(n => n.NoteTags)
                                              .ThenInclude(nt => nt.Tag)
                                              .FirstOrDefaultAsync(n => n.NoteId == updatedNote.NoteId);

            if (existingNote == null)
            {
                throw new KeyNotFoundException("Note not found");
            }

            existingNote.Title = updatedNote.Title;
            existingNote.Content = updatedNote.Content;
            existingNote.UpdatedAt = DateTime.UtcNow;

            existingNote.NoteTags.Clear();
            foreach (var tagName in updatedNote.Tags)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName)
                           ?? new Tag { Name = tagName };

                existingNote.NoteTags.Add(new NoteTag { Note = existingNote, Tag = tag });
            }

            await _context.SaveChangesAsync();

            return new updateNoteDto
            {
                NoteId = existingNote.NoteId,
                Title = existingNote.Title,
                Content = existingNote.Content,
                UpdatedAt = existingNote.UpdatedAt,
                Tags = updatedNote.Tags 
            };
        }

        public async Task<bool> DeleteNoteAsync(int noteId)
        {
            var note = await _context.Notes.FindAsync(noteId);

            if (note == null)
            {
                return false;
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
