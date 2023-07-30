using Moq;
using ParkinApp.Domain.Abstractions.Repositories;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Domain.DTOs;
using ParkinApp.Domain.Entities;
using ParkinApp.Services;
using Shouldly;

namespace ParkinApp.UnitTests.Services
{
    public class ReservationServiceTests
    {
        private readonly Mock<IReservationRepository> _reservationRepositoryMock;
        private readonly Mock<IParkingSpotRepository> _parkingSpotRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IParkingSpotCacheService> _parkingSpotCacheServiceMock;

        public ReservationServiceTests()
        {
            _reservationRepositoryMock = new Mock<IReservationRepository>();
            _parkingSpotRepositoryMock = new Mock<IParkingSpotRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _parkingSpotCacheServiceMock = new Mock<IParkingSpotCacheService>();
        }

        [Fact]
        public async Task CreateReservationAsync_WhenParkingSpotIsAvailable_CreatesReservationAndUpdatesParkingSpot()
        {
            // Arrange
            var parkingSpotId = 1;
            var userId = 1;
            var username = "testUser";
            var parkingSpot = new ParkingSpot { Id = parkingSpotId };
            var user = new User { Id = userId };

            _userRepositoryMock.Setup(u => u.GetUserByUsernameAsync(username)).ReturnsAsync(user);
            _parkingSpotCacheServiceMock.Setup(p => p.GetParkingSpotByIdAsync(parkingSpotId, It.IsAny<IParkingSpotRepository>())).ReturnsAsync(parkingSpot);
            _reservationRepositoryMock.Setup(r => r.GetActiveReservationByUserIdAsync(userId)).ReturnsAsync((Reservation)null!);

            var reservationService = new ReservationService(
                _parkingSpotRepositoryMock.Object,
                _userRepositoryMock.Object,
                _reservationRepositoryMock.Object,
                _parkingSpotCacheServiceMock.Object);

            // Act
            var reservationDto = new CreateReservationDto(parkingSpotId);
            var result = await reservationService.CreateReservationAsync(reservationDto, username);

            // Assert
            result.IsSuccessful.ShouldBeTrue();

            _reservationRepositoryMock.Verify(r => r.AddAsync(It.Is<Reservation>(r =>
                r.ParkingSpotId == parkingSpotId &&
                r.UserId == userId)), Times.Once);

            _parkingSpotCacheServiceMock.Verify(p => p.UpdateParkingSpotCache(parkingSpot), Times.Once);
        }
        
        [Fact]
        public async Task CreateReservationAsync_WhenUserHasActiveReservation_ReturnsFailure()
        {
            // Arrange
            var parkingSpotId = 1;
            var userId = 1;
            var username = "testUser";
            var user = new User { Id = userId };
            var activeReservation = new Reservation();

            _userRepositoryMock.Setup(u => u.GetUserByUsernameAsync(username)).ReturnsAsync(user);
            _reservationRepositoryMock.Setup(r => r.GetActiveReservationByUserIdAsync(userId)).ReturnsAsync(activeReservation);

            var reservationService = new ReservationService(
                _parkingSpotRepositoryMock.Object,
                _userRepositoryMock.Object,
                _reservationRepositoryMock.Object,
                _parkingSpotCacheServiceMock.Object);

            // Act
            var reservationDto = new CreateReservationDto(parkingSpotId);
            var result = await reservationService.CreateReservationAsync(reservationDto, username);

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.Errors.ShouldContain("User already has an active reservation.");
        }
        
        [Fact]
        public async Task CreateReservationAsync_WhenParkingSpotDoesNotExist_ReturnsFailure()
        {
            // Arrange
            var parkingSpotId = 1;
            var userId = 1;
            var username = "testUser";
            var user = new User { Id = userId };

            _userRepositoryMock.Setup(u => u.GetUserByUsernameAsync(username)).ReturnsAsync(user);
            _parkingSpotCacheServiceMock.Setup(p => p.GetParkingSpotByIdAsync(parkingSpotId, It.IsAny<IParkingSpotRepository>())).ReturnsAsync((ParkingSpot)null);

            var reservationService = new ReservationService(
                _parkingSpotRepositoryMock.Object,
                _userRepositoryMock.Object,
                _reservationRepositoryMock.Object,
                _parkingSpotCacheServiceMock.Object);

            // Act
            var reservationDto = new CreateReservationDto(parkingSpotId);
            var result = await reservationService.CreateReservationAsync(reservationDto, username);

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.Errors.ShouldContain("Parking spot not found.");
        }
        
       
    }
}
