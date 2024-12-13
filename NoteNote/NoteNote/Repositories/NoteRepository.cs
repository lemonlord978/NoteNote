using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NoteNote.DBContext;
using NoteNote.Dtos;
using NoteNote.Models;
using NoteNote.Repositories.IReposotories;
using System.ComponentModel.DataAnnotations;

namespace NoteNote.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly NoteAppContext _context;
        private readonly IMapper _mapper;
        public NoteRepository(NoteAppContext context, IMapper mapper)
        {
            _mapper = mapper;
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
        public List<ViewNoteDto> GetNotesQueryable(int userId, string searchQuery)
        {
            var query = _context.Notes
                .Include(note => note.NoteTags)
                .ThenInclude(nt => nt.Tag)
                .Where(note => note.UserId == userId);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(note =>
                    note.Title.Contains(searchQuery) ||
                    note.NoteTags.Any(nt => nt.Tag.Name.Contains(searchQuery))
                );
            }

            var notes = query.ToList();
            return _mapper.Map<List<ViewNoteDto>>(notes);
        }

        public async Task<NoteDto> CreateNoteAsync(NoteDto note)
        {
            var data = _mapper.Map<Note>(note);

            // Validate
            var validationContext = new ValidationContext(data);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(data, validationContext, validationResults, true))
            {
                var errorMessages = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                throw new ArgumentException($"Validation failed: {errorMessages}");
            }

            await _context.Notes.AddAsync(data);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<ViewNoteDto> ViewNoteById(int noteId)
        {
            var note = await _context.Notes
                                     .Include(p => p.NoteTags)
                                         .ThenInclude(nt => nt.Tag)
                                     .FirstOrDefaultAsync(p => p.NoteId == noteId);

            if (note == null)
            {
                return null;
            }

            var data = _mapper.Map<ViewNoteDto>(note);

            return data;
        }

        public async Task<ViewNoteDto> ViewNewestNoteByUserId(int userId)
        {
            var note = await _context.Notes
                                 .Where(p => p.UserId == userId)
                                 .OrderByDescending(p => p.CreatedAt)
                                 .FirstOrDefaultAsync();
            var data = _mapper.Map<ViewNoteDto>(note);
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

            // Validate
            var validationContext = new ValidationContext(existingNote);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(existingNote, validationContext, validationResults, true))
            {
                _context.Entry(existingNote).State = EntityState.Unchanged;
                var errorMessages = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                throw new ArgumentException($"Validation failed: {errorMessages}");
            }

            await _context.SaveChangesAsync();
            return updatedNote;
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
