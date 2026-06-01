using AltaMesa.web.Constants;
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
            var vm = await ObtenerDashboard();
            return View(vm);
        }

        [HttpGet]
        public async Task<JsonResult> GetCounters()
        {
            var vm = await ObtenerDashboard();
            return Json(new
            {
                mesasOcupadas = vm.MesasOcupadas,
                mesasDisponibles = vm.MesasDisponibles,
                pedidosActivos = vm.PedidosActivos,
                platosEnPreparacion = vm.PlatosEnPreparacion,
                platosListos = vm.PlatosListos
            }, JsonRequestBehavior.AllowGet);
        }

        private async Task<DashboardVM> ObtenerDashboard()
        {
            var mesas = await _mesaService.Listar();
            var pedidos = await _pedidoService.ListarActivos();
            var colaCocina = await _pedidoService.ListarColaCocina();

            return new DashboardVM
            {
                MesasOcupadas = mesas.Count(m => m.Estado == "Ocupada"),
                MesasDisponibles = mesas.Count(m => m.Estado == "Disponible"),
                PedidosActivos = pedidos.Count,
                PlatosEnPreparacion = colaCocina.Count(c =>
                    c.EstadoDetalle == DetalleEstado.Ingresado ||
                    c.EstadoDetalle == DetalleEstado.EnPreparacion),
                PlatosListos = colaCocina.Count(c =>
                    c.EstadoDetalle == DetalleEstado.ListoParaServir),
                UltimosPedidos = pedidos
                    .OrderByDescending(p => p.FechaPedido)
                    .Take(5)
                    .ToList()
            };
        }
    }
}
