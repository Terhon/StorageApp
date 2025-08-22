using Microsoft.AspNetCore.SignalR;

namespace Storage.Api;

public class TestHub: Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}