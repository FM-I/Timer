using Microsoft.AspNetCore.SignalR;
using Server.Common;
using Server.Interfaces;

namespace Server.Hubs
{
    public class TimerHub : Hub
    {
        private readonly IConnectionsRepository _connectionsRepository;
        private readonly TimerManager _timerManager;

        public TimerHub(IConnectionsRepository connectionsRepository, TimerManager timerManager)
        {
            _connectionsRepository = connectionsRepository;
            _timerManager = timerManager;
        }

        public async Task Join(string groupName)
        {
            var data = _connectionsRepository.Get(Context.ConnectionId);

            if (data == null)
            {
                data = new(Context.ConnectionId);
            }
            
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            data.GroupName = groupName;
            _connectionsRepository.Update(data);
        }

        public void TimerAction()
        {
            var data = _connectionsRepository.Get(Context.ConnectionId);

            if(data != null)
            {
                _timerManager.Start(data);
            }
        }

        public async Task Leave()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "kva");
            _connectionsRepository.Remove(Context.ConnectionId);
        }

        public override Task OnConnectedAsync()
        {
            _connectionsRepository.Add(new(Context.ConnectionId));
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _connectionsRepository.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
