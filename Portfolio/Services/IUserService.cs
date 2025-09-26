using Microsoft.EntityFrameworkCore;
using Portfolio.Models;

namespace Portfolio.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByGoogleIdAsync(string googleId);
        Task<User> CreateUserAsync(string email, string displayName, string? googleId = null);
        Task<bool> ValidatePasswordAsync(string email, string password);
    }

    public class UserService : IUserService
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        // Simple in-memory storage for demo purposes (replace with proper hashing)
        private static readonly Dictionary<string, string> _passwords = new();

        public UserService(Data.ApplicationDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByGoogleIdAsync(string googleId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId);
        }

        public async Task<User> CreateUserAsync(string email, string displayName, string? googleId = null)
        {
            var user = new User
            {
                Email = email,
                DisplayName = displayName,
                GoogleId = googleId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created new user: {Email}", email);
            return user;
        }

        public Task<bool> ValidatePasswordAsync(string email, string password)
        {
            // Simple demo validation - replace with proper password hashing
            if (_passwords.TryGetValue(email, out var storedPassword))
            {
                return Task.FromResult(storedPassword == password);
            }
            return Task.FromResult(false);
        }

        public void StorePassword(string email, string password)
        {
            _passwords[email] = password;
        }
    }
}