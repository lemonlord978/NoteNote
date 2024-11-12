using NoteNote.Dtos;
using NoteNote.Models;

namespace NoteNote.Services.IServices
{
    public interface IUserService
    {
        Task<User> RegisterAsync(string username, string password, string email);
        Task<User> GetUserByUsernameAsync(string username);
        Task<getUserDto> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> LoginAsync(string username, string password);
    }
}
