using ParkinApp.Domain.Entities;

namespace ParkinApp.Domain.Abstractions.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByUsername(string username);
        Task<bool> UserExists(string username);
        Task UpdateUserAsync(User user);
    }
}