using AutoMapper;
using NoteNote.Dtos;
using NoteNote.Models;

namespace NoteNote.MappingProfile.NoteProfile
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<Note, ViewNoteDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.NoteTags.Select(nt => nt.Tag.Name)));
            
            CreateMap<Note, NoteDto>().ReverseMap();
        }
    }
}
