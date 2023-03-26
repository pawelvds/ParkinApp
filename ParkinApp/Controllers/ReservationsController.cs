using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.DTOs;

namespace ParkinApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto reservationDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null)
            {
                var result = await _reservationService.CreateReservationAsync(reservationDto, userId);

                if (result.IsSuccessful)
                {
                    return Ok(result.Value);
                }

                return BadRequest(result.Errors);
            }

            return Unauthorized();
        }


        [HttpDelete("cancel")]
        public async Task<IActionResult> CancelReservation()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _reservationService.CancelReservationAsync(userId);

            if (result.IsSuccessful)
            {
                return Ok(result.Value);
            }

            return Unauthorized(result.Errors);
        }
    }
}