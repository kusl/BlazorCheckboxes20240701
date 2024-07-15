using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BlazorCheckboxes;

public class CounterHub(CounterContext context, ILogger<CounterHub> logger) : Hub
{
    public async Task SendMessage(string message)
    {
        var counter = await context.Counters.FirstOrDefaultAsync() ?? new Counter { Value = 0 };
        logger.LogInformation("{Module}: In {class} in {method}, {action} with {parameters}", "Hub", nameof(CounterHub), nameof(SendMessage), "original value for counter", counter.Value.ToString());

        if (message == "increment")
        {
            counter.Value++;
        }
        else if (message == "decrement")
        {
            counter.Value--;
        }
        else if (message == "multiplyByRandomNumber")
        {
            int randomNumber = Random.Shared.Next(1, 1000);
            counter.Value *= randomNumber;
            if (counter.Value == 0)
            {
                counter.Value++;
            }
        }
        else if (message == "resetToZero")
        {
            counter.Value = 0;
        }

        if (counter.Id == 0)
        {
            context.Counters.Add(counter);
        }
        else
        {
            context.Counters.Update(counter);
        }
        logger.LogInformation("{Module}: In {class} in {method}, {action} with {parameters}", "Hub", nameof(CounterHub), nameof(SendMessage), "new value of counter to save", counter.Value.ToString());
        await context.SaveChangesAsync();
        logger.LogInformation("{Module}: In {class} in {method}, {action} with {parameters}", "Hub", nameof(CounterHub), nameof(SendMessage), "new value of counter saved", counter.Value.ToString());
        await Clients.All.SendAsync("ReceiveMessage", counter.Value.ToString());
        logger.LogInformation("{Module}: In {class} in {method}, {action} with {parameters}", "Hub", nameof(CounterHub), nameof(SendMessage), "ReceiveMessage", counter.Value.ToString());
    }

    public override async Task OnConnectedAsync()
    {
        var counter = await context.Counters.FirstOrDefaultAsync() ?? new Counter { Value = 0 };
        await Clients.Caller.SendAsync("ReceiveMessage", counter.Value.ToString());
        logger.LogInformation("{Module}: In {class} in {method}, {action} with {parameters}", "Hub", nameof(CounterHub), nameof(OnConnectedAsync), "ReceiveMessage", counter.Value.ToString());
        await base.OnConnectedAsync();
    }
}