using Microsoft.AspNetCore.SignalR;
using Server.Hubs;
using Server.Interfaces;
using Server.Models;

namespace Server.Common
{
    public class TimerManager
    {
        private readonly IHubContext<TimerHub> _hubContext;
        private Dictionary<string, System.Timers.Timer> _timers;
        private Dictionary<int, int> _counters;
        private readonly IConnectionsRepository _connectionsRepository;

        public TimerManager(IHubContext<TimerHub> hubContext, IConnectionsRepository connectionsRepository)
        {
            _timers = new();
            _counters = new();
            _hubContext = hubContext;
            _connectionsRepository = connectionsRepository;
        }

        public void Start(ConnectionData connectionData)
        {
            string key = connectionData.GroupName!;
            System.Timers.Timer? timer;

            if(!_timers.TryGetValue(key, out timer))
            {
                timer = new System.Timers.Timer(1000);
                
                _counters.Add(timer.GetHashCode(), 0);

                timer.Elapsed += async (sender, e) =>
                {
                    var counter = _counters.GetValueOrDefault(timer.GetHashCode());
                    ++counter;
                    _counters.Remove(timer.GetHashCode());
                    _counters.Add(timer.GetHashCode(), counter);

                    await _hubContext.Clients.Group(key).SendAsync("Timer", counter);

                    if (counter == 10)
                    {
                        _timers.Remove(key);
                        _counters.Remove(timer.GetHashCode());
                        timer.Stop();
                        timer.Dispose();

                        List<string> connections = _connectionsRepository.Where(x => x.GroupName == key && x.Id != connectionData.Id).Select(s => s.Id).ToList();

                        await _hubContext.Clients.GroupExcept(key, connectionData.Id).SendAsync("Leave", "Kvaa you leave");
                        foreach (var connection in connections)
                        {
                            await _hubContext.Groups.RemoveFromGroupAsync(connection, key);
                        }
                        return;
                    }
                };

                _timers.Add(key, timer);

                timer.Start();
            }

        }
    }
}
