using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace AltaMesa.web.Hubs
{
    public class PedidoHub : Hub
    {
        public override async Task OnConnected()
        {
            await Groups.Add(Context.ConnectionId, "pedidos").ConfigureAwait(false);
            await base.OnConnected().ConfigureAwait(false);
        }

        public Task JoinGroup()
        {
            return Groups.Add(Context.ConnectionId, "pedidos");
        }
    }
}
