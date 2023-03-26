using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.DTOs;
using ParkinApp.Persistence.Data;
using ParkinApp.Validators;

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
    public async Task<IActionResult> GetParkingSpots([CustomizeValidator] GetParkingSpotsRequestValidator validator)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var parkingSpots = await _parkingSpotService.GetAllParkingSpotsAsync();
        return Ok(parkingSpots);
    }
}

