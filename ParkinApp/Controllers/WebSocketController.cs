using System.Text;
using ParkinApp.Domain.Abstractions.Services;
using ParkinApp.Domain.Common;
using ParkinApp.Domain.DTOs;
using ParkinApp.Services;
using FluentValidation.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ParkinApp.Controllers
{
    public class WebSocketController
    {
        private readonly ILogger<WebSocketController> _logger;
        private readonly IReservationService _reservationService;

        public WebSocketController(ILogger<WebSocketController> logger, IReservationService reservationService)
        {
            _logger = logger;
            _reservationService = reservationService;
        }

        public async Task HandleWebSocketAsync(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;
            do
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    _logger.LogInformation($"Received message: {message}");

                    // Deserialize the message
                    var webSocketMessage = JsonConvert.DeserializeObject<WebSocketMessage>(message);

                    // Handle different actions
                    string responseMessage = string.Empty;
                    switch (webSocketMessage.Action)
                    {
                        case "reserve":
                            // Perform reservation action
                            var reservationDto = webSocketMessage.Data.ToObject<CreateReservationDto>();
                            var userId = context.User.Identity.Name;
                            var reserveResult = await _reservationService.CreateReservationAsync(reservationDto, userId);
                            if (reserveResult.IsSuccessful)
                            {
                                responseMessage = "Reservation successful";
                            }
                            else
                            {
                                responseMessage = "Reservation failed: " + string.Join(", ", reserveResult.Errors);
                            }
                            break;
                        case "cancel":
                            // Perform cancellation action
                            userId = context.User.Identity.Name;
                            var cancelResult = await _reservationService.CancelReservationAsync(userId);
                            if (cancelResult.IsSuccessful)
                            {
                                responseMessage = "Reservation cancelled";
                            }
                            else
                            {
                                responseMessage = "Cancellation failed: " + string.Join(", ", cancelResult.Errors);
                            }
                            break;
                        default:
                            _logger.LogWarning($"Unknown action: {webSocketMessage.Action}");
                            break;
                    }

                    // Send response to the client
                    var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                    await webSocket.SendAsync(new ArraySegment<byte>(responseBuffer, 0, responseBuffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            while (!result.CloseStatus.HasValue);

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }

    public class WebSocketMessage
    {
        public string Action { get; set; }
        public JObject Data { get; set; }
    }
}
