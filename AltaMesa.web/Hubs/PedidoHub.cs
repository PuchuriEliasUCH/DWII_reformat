using Microsoft.AspNet.SignalR;

namespace AltaMesa.web.Hubs
{
    public class PedidoHub : Hub
    {
        public void JoinGroup()
        {
            Groups.Add(Context.ConnectionId, "pedidos");
        }
    }
}
