﻿namespace NoteNote.Models
{
    public class NoteTag
    {
        public int NoteId { get; set; }
        public Note Note { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }

}
