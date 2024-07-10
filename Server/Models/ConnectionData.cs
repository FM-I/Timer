namespace Server.Models
{
    public class ConnectionData
    {
        public string Id { get; private set; } = string.Empty;
        public string? GroupName { get; set; }

        public ConnectionData(string id)
        {
            Id = id;
        }

    }
}
