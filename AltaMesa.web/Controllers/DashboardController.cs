using AltaMesa.web.Filters;
using AltaMesa.web.Models.ViewModels;
using AltaMesa.web.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AltaMesa.web.Controllers
{
    [AutorizarRol(Rol = "admin")]
    public class DashboardController : Controller
    {
        private readonly IMesaService _mesaService;
        private readonly IPedidoService _pedidoService;

        public DashboardController(
            IMesaService mesaService,
            IPedidoService pedidoService)
        {
            _mesaService = mesaService;
            _pedidoService = pedidoService;
        }

        public async Task<ActionResult> Index()
        {
            var mesas = await _mesaService.Listar();
            var pedidos = await _pedidoService.ListarActivos();
            var colaCocina = await _pedidoService.ListarColaCocina();

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
