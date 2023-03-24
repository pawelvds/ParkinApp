using Microsoft.AspNetCore.Mvc;
using ParkinApp.DTOs;
using ParkinApp.Persistence.Data;

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
    public async Task<IActionResult> GetParkingSpots()
    {
        var parkingSpots = _context.ParkingSpots
            .Select(ps => new ParkingSpotDto
            {
                Id = ps.Id,
                IsReserved = ps.IsReserved //////
            })
            .ToList();

        return Ok(parkingSpots);
    }
}

