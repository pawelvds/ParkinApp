using ParkinApp.Domain.Entities;

namespace ParkinApp.Domain.Abstractions.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> UserExists(string username);
        Task<User?> GetUserByRefreshToken(string refreshToken);
        Task<User?> GetUserByUsernameAsync(string username);
    }
}