using Server.Interfaces;
using Server.Models;

namespace Server.Repositories
{
    public class ConnectionsRepository : IConnectionsRepository
    {
        private List<ConnectionData> _connections;

        public ConnectionsRepository()
        {
            _connections = new();
        }

        public void Add(ConnectionData connection)
        {
            _connections.Add(connection);
        }

        public ConnectionData? Get(string connectionId)
        {
            var item = _connections.FirstOrDefault(x => x.Id == connectionId);
            return item;
        }

        public void Remove(string connectionId)
        {
            var item = _connections.FirstOrDefault(x => x.Id == connectionId);

            if (item != null)
            {
                _connections.Remove(item);
            }
        }

        public void Update(ConnectionData connection)
        {
            var item = _connections.FirstOrDefault(x => x.Id == connection.Id);

            if (item != null)
            {
                item.GroupName = connection.GroupName;
                _connections.Remove(item);
                Add(item);
            }
            else
            {
                Add(connection);
            }
        }

        public string? GetGroupName(string connectionId)
        {
            var item = _connections.FirstOrDefault(x => x.Id == connectionId);

            if (item != null)
            {
                return item.GroupName;
            }

            return null;
        }

        public List<ConnectionData> Where(Func<ConnectionData, bool> where)
        {
            return _connections.Where(where).ToList();
        }
    }
}
