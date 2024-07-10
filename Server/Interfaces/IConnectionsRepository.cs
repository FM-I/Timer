using Server.Models;

namespace Server.Interfaces
{
    public interface IConnectionsRepository
    {
        void Add(ConnectionData connection);
        ConnectionData? Get(string connectionId);
        public void Update(ConnectionData connection);
        void Remove(string connectionId);
        string? GetGroupName(string connectionId);
        public List<ConnectionData> Where(Func<ConnectionData, bool> where);
    }
}