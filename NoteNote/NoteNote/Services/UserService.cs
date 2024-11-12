using NoteNote.Dtos;
using NoteNote.Models;
using NoteNote.Repositories.IReposotories;
using NoteNote.Services.IServices;

namespace NoteNote.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _userRepository.GetUserByEmailAsync(email);
        }

        public Task<User> GetUserByUsernameAsync(string username)
        {
            return _userRepository.GetUserByUsernameAsync(username);
        }

        public Task<getUserDto> GetUserByIdAsync(int id)
        {
            return _userRepository.GetUserByIdAsync(id);
        }

        public Task<User> LoginAsync(string username, string password)
        {
            return _userRepository.LoginAsync(username, password);
        }

        public Task<User> RegisterAsync(string username, string password, string email)
        {
            return _userRepository.RegisterAsync(username, password, email);
        }
    }
}
