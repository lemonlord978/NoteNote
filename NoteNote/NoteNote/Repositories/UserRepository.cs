using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteNote.DBContext;
using NoteNote.Dtos;
using NoteNote.Models;
using NoteNote.Repositories.IReposotories;
using System.ComponentModel.DataAnnotations;

namespace NoteNote.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NoteAppContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        public UserRepository(NoteAppContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<User> RegisterAsync(string username, string password, string email)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
            {
                throw new ArgumentException("Username already taken.");
            }

            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                throw new ArgumentException("Email already taken.");
            }

            var user = new User
            {
                Username = username,
                Password = password,
                Email = email,
                CreatedAt = DateTime.Now
            };

            // Validate the user model
            var validationContext = new ValidationContext(user);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(user, validationContext, validationResults, true))
            {
                var errorMessages = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                throw new ArgumentException($"Validation failed: {errorMessages}");
            }

            user.Password = GetHashPassword(password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private string GetHashPassword(string password)
        {
            var hashedPassword = _passwordHasher.HashPassword(null, password);
            return hashedPassword;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<getUserDto> GetUserByIdAsync(int id)
        {
            var data = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            var output = new getUserDto {
                Username = data.Username };
            return output;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return user;
        }
    }
}
