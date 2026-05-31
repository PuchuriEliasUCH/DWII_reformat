using System.Linq;
using System.Web.Mvc;
using AltaMesa.web.Filters;
using AltaMesa.web.Models.ViewModels;
using AltaMesa.web.Services;

namespace AltaMesa.web.Controllers
{
    [AutorizarRol(Rol = "Administrador")]
    public class DashboardController : Controller
    {
        private readonly MesaService _mesaService;
        private readonly PedidoService _pedidoService;

        public DashboardController()
        {
            _mesaService = new MesaService();
            _pedidoService = new PedidoService();
        }

        public ActionResult Index()
        {
            var mesas = _mesaService.Listar();
            var pedidos = _pedidoService.ListarActivos();
            var colaCocina = _pedidoService.ListarColaCocina();

            var vm = new DashboardVM
            {
                MesasOcupadas = mesas.Count(m => m.Estado == "Ocupada"),
                MesasDisponibles = mesas.Count(m => m.Estado == "Disponible"),
                PedidosActivos = pedidos.Count,
                ProductosPendientes = colaCocina.Count,
                UltimosPedidos = pedidos.OrderByDescending(p => p.FechaPedido).Take(5).ToList()
            };

            return View(vm);
        }
    }
}
