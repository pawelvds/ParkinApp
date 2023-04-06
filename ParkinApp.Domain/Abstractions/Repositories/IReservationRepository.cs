using ParkinApp.Domain.Entities;

namespace ParkinApp.Domain.Abstractions.Repositories
{
    public interface IReservationRepository : IGenericRepository<Reservation>
    {
        Task<Reservation?>GetActiveReservationByUserIdAsync(int userId);
    }
}