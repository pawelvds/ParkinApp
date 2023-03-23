using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkinApp.Data;
using ParkinApp.Interfaces;
using System.Security.Claims;

namespace ParkinApp.Controllers;

[Authorize]
[Route("api/Reservations")]
public class ReservationsController : ControllerBase
{
    private readonly ParkingDbContext _context;
    
    private readonly ITokenService _tokenService;
    private const string CentralEuropeanTimeZone = "Europe/Warsaw";

    public ReservationsController(ParkingDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("{id}")]
    public async Task<ActionResult> CreateReservation(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized("Invalid user.");
        }

        var user = _context.Users.FirstOrDefault(u => u.Login == userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (user.ReservedSpotId != null)
        {
            return BadRequest("User already has a reserved parking spot.");
        }

        var parkingSpot = await _context.ParkingSpots.FindAsync(id);
        if (parkingSpot == null)
        {
            return NotFound("Parking spot not found.");
        }

        if (parkingSpot.IsReserved)
        {
            return BadRequest("Parking spot is already reserved.");
        }

        parkingSpot.IsReserved = true;
        parkingSpot.UserId = user.Id;

        var userTimeZoneId = user.UserTimeZoneId;

        var spotTimeZone = TimeZoneInfo.FindSystemTimeZoneById(parkingSpot.SpotTimeZoneId);
        var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, spotTimeZone);
        parkingSpot.ReservationTime = localNow;

        DateTime reservationEndTime = new DateTime(localNow.Year, localNow.Month, localNow.Day, 23, 59, 59, DateTimeKind.Local);
        reservationEndTime = TimeZoneInfo.ConvertTime(reservationEndTime, spotTimeZone);
        parkingSpot.ReservationEndTime = reservationEndTime;

        parkingSpot.UserTimeZoneId = userTimeZoneId;

        user.ReservedSpotId = parkingSpot.Id;

        await _context.SaveChangesAsync();

        return Ok("Parking spot reserved.");
        
    }


    [HttpDelete("cancelReservation")]
    public async Task<IActionResult> CancelReservation()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized("Invalid user.");
        }

        var user = _context.Users.FirstOrDefault(u => u.Login == userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (user.ReservedSpotId == null)
        {
            return BadRequest("User doesn't have any reserved spot.");
        }

        var parkingSpot = _context.ParkingSpots.FirstOrDefault(ps => ps.Id == user.ReservedSpotId);
        if (parkingSpot == null)
        {
            return NotFound("Reserved parking spot not found.");
        }
        
        // Cancel reservation
        parkingSpot.IsReserved = false;
        parkingSpot.UserId = null;
        parkingSpot.ReservationTime = null;
        parkingSpot.ReservationEndTime = null;

        // Update user reservation
        user.ReservedSpotId = null;

        // Update the user's time zone in the parking spot table
        parkingSpot.UserTimeZoneId = parkingSpot.SpotTimeZoneId;

        await _context.SaveChangesAsync();

        return Ok("Reservation cancelled.");
    }

}
