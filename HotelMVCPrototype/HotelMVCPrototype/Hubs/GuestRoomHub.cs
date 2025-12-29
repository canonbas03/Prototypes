using Microsoft.AspNetCore.SignalR;

public class GuestRoomHub : Hub
{
    // Method to notify all connected clients that a room's DND changed
    public async Task RoomDndToggled(int roomId, bool isDnd)
    {
        await Clients.All.SendAsync("ReceiveDndUpdate", roomId, isDnd);
    }
}
