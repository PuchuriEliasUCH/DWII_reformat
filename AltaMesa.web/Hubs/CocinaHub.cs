using Microsoft.AspNet.SignalR;

namespace AltaMesa.web.Hubs
{
    public class CocinaHub : Hub
    {
        public void JoinGroup()
        {
            Groups.Add(Context.ConnectionId, "cocina");
        }
    }
}
