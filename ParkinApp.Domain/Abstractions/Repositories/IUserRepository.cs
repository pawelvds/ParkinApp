using ParkingApp.Entities;

namespace ParkinApp.Domain.Abstractions.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByUsername(string username);
        Task<bool> UserExists(string username);
    }
}