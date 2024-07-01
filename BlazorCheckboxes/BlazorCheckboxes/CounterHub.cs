using Microsoft.AspNetCore.SignalR;

public class CounterHub : Hub
{
    private static int _currentCount = 0;

    public async Task SendMessage(string message)
    {
        if (message == "increment")
        {
            _currentCount++;
        }
        else if (message == "decrement")
        {
            _currentCount--;
        }
        else if (message == "multiplyByRandomNumber")
        {
            int randomNumber = Random.Shared.Next(1, 1000);
            _currentCount *= randomNumber;
        }
        else if (message == "resetToZero")
        {
            _currentCount = 0;
        }
        await Clients.All.SendAsync("ReceiveMessage", _currentCount.ToString());
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("ReceiveMessage", _currentCount.ToString());
        await base.OnConnectedAsync();
    }
}
