namespace NoteNote.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }

        public ICollection<NoteTag> NoteTags { get; set; } = new List<NoteTag>();
    }

}
