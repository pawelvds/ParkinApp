using Microsoft.AspNetCore.SignalR;

namespace ParkinApp.Hubs;

public class ParkingSpotHub : Hub
{
    public async Task SendParkingSpotStatus(string parkingSpotId, bool isAvailable)
    {
        await Clients.All.SendAsync("ReceiveParkingSpotStatus", parkingSpotId, isAvailable);
    }
}