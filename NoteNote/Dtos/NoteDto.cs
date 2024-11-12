using NoteNote.Models;

namespace NoteNote.Dtos
{
    public class NoteDto
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
    
    public class NoteViewDto
    {
        public int UserId { get; set; }
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
    }
    
    public class ViewNoteDto
    {
        public int NoteId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Tags { get; set; }
    }

    public class updateNoteDto
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginatedResult(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
