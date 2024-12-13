using System.ComponentModel.DataAnnotations;

namespace NoteNote.Models
{
    public class Note
    {
        public int NoteId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<NoteTag> NoteTags { get; set; } = new List<NoteTag>();
    }

}
