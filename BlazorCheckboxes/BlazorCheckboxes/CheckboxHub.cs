using Microsoft.AspNetCore.SignalR;

namespace BlazorCheckboxes
{
    public class CheckboxHub(ILogger<CheckboxHub> logger) : Hub
    {
        private static readonly Dictionary<string, bool> _checkboxStatuses = InitializeCheckboxStatuses(10000);

        private static Dictionary<string, bool> InitializeCheckboxStatuses(int count)
        {
            var statuses = new Dictionary<string, bool>();
            for (int i = 0; i < count; i++)
            {
                statuses.Add($"checkbox{i}", false);
            }
            return statuses;
        }

        public async Task SendMessage(Dictionary<string, bool> checkboxUpdates)
        {
            try
            {
                foreach (var update in checkboxUpdates)
                {
                    if (_checkboxStatuses.ContainsKey(update.Key))
                    {
                        _checkboxStatuses[update.Key] = update.Value;
                    }
                }
                await Clients.All.SendAsync("ReceiveMessage", _checkboxStatuses);
                logger.LogInformation("Broadcasted message: {Statuses}", string.Join(", ", _checkboxStatuses.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in SendMessage");
            }
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                await Clients.Caller.SendAsync("ReceiveMessage", _checkboxStatuses);
                await base.OnConnectedAsync();
                logger.LogInformation("Client connected. Sent initial state.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in OnConnectedAsync");
            }
        }
    }
}
