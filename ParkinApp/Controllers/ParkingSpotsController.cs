using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using ParkinApp.Data;
using ParkinApp.DTOs;


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
}

