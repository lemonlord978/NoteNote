using System.ComponentModel.DataAnnotations;

namespace NoteNote.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(8, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 8 characters")]
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Note> Notes { get; set; } = new List<Note>();
    }
}
