using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using ParkinApp.DTOs;
using ParkingApp.Data;


namespace ParkinApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingSpotsController : ControllerBase
{
    private readonly ParkingDbContext _context;

    public ParkingSpotsController(ParkingDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetParkingSpots()
    {
        var parkingSpots = _context.ParkingSpots
            .Select(ps => new ParkingSpotDto
            {
                Id = ps.Id,
                IsReserved = ps.IsReserved
            })
            .ToList();

        return Ok(parkingSpots);
    }

    [HttpPost("{id}/reserve")]
    public IActionResult ReserveSpot(int id)
    {
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

        _context.SaveChanges();

        return Ok("Parking spot reserved.");
    }

    [HttpPut("{id}/cancel")]
    public IActionResult CancelReservation(int id)
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

