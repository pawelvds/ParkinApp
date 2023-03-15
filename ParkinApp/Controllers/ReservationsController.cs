using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkinApp.Data;
using ParkinApp.DTOs;
using ParkinApp.Interfaces;
using ParkingApp.Entities;

namespace ParkinApp.Controllers;

[Authorize]
public class ReservationsController : ControllerBase
{
    private readonly ParkingDbContext _context;
    
    private readonly ITokenService _tokenService;

    public ReservationsController(ParkingDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("{id}")]
    public IActionResult CreateReservation(int id, [FromBody] UserDto userCredentials)
    {
        var user = _context.Users.FirstOrDefault(u => u.Login == userCredentials.Username);

        if (user == null)
        {
            return Unauthorized("Invalid user.");
        }

        if (user.ReservedSpot != null)
        {
            return BadRequest("User already has a reservation.");
        }

        var parkingSpot = _context.ParkingSpots.FirstOrDefault(ps => ps.Id == id);

        if (parkingSpot == null)
        {
            return NotFound("Parking spot not found.");
        }

        if (parkingSpot.IsReserved)
        {
            return BadRequest("Parking spot is already reserved.");
        }

        parkingSpot.IsReserved = true;
        parkingSpot.ReservationTime = DateTime.UtcNow;
        parkingSpot.UserId = user.Id;

        _context.SaveChanges();

        return Ok("Parking spot reserved.");
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteReservation(int id)
    {
        var parkingSpot = _context.ParkingSpots.FirstOrDefault(ps => ps.Id == id);

        if (parkingSpot == null)
        {
            return NotFound("Parking spot not found.");
        }

        if (!parkingSpot.IsReserved)
        {
            return BadRequest("Parking spot is not reserved.");
        }

        parkingSpot.IsReserved = false;
        parkingSpot.ReservationTime = null;

        _context.SaveChanges();

        return Ok("Reservation canceled.");
    }
}
