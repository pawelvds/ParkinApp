using System.Threading.Tasks;
using ParkingApp.Entities;

namespace ParkinApp.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
        Task<bool> UserExists(string username);
    }
}