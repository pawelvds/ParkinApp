using Microsoft.AspNetCore.Mvc;
using ParkinApp.Domain.Abstractions.Services;

namespace ParkinApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingSpotsController : ControllerBase
{
    private readonly IParkingSpotService _parkingSpotService;

    public ParkingSpotsController(IParkingSpotService parkingSpotService)
    {
        _parkingSpotService = parkingSpotService;
    }

    [HttpGet]
    public async Task<IActionResult> GetParkingSpots()
    {
        var parkingSpots = await _parkingSpotService.GetAllParkingSpotsAsync();
        return Ok(parkingSpots);
    }
}


