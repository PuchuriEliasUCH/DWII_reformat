using AltaMesa.web.Constants;
using AltaMesa.web.Filters;
using AltaMesa.web.Helpers;
using AltaMesa.web.Models.ViewModels;
using AltaMesa.web.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AltaMesa.web.Controllers
{
    [AutorizarRol(Rol = "chef")]
    public class CocinaController : Controller
    {
        private readonly IPedidoService _pedidoService;
        private readonly INotificationService _notificationService;

        public CocinaController(
            IPedidoService pedidoService,
            INotificationService notificationService)
        {
            _pedidoService = pedidoService;
            _notificationService = notificationService;
        }

        public async Task<ActionResult> Index()
        {
            var cola = await _pedidoService.ListarColaCocina();
            var vm = new CocinaListaVM
            {
                NuevosItems = cola.Where(c => c.EstadoDetalle == DetalleEstado.Ingresado).ToList(),
                EnPreparacion = cola.Where(c => c.EstadoDetalle == DetalleEstado.EnPreparacion).ToList(),
                ProductosListos = cola.Where(c => c.EstadoDetalle == DetalleEstado.ListoParaServir).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> CambiarEstado(int detalleId, string estado)
        {
            try
            {
                var usuarioId = SessionHelper.GetUsuarioId().GetValueOrDefault();
                await _pedidoService.CambiarEstadoDetalle(detalleId, estado, usuarioId);

                var detalles = await _pedidoService.ListarColaCocina();
                var detalle = detalles.FirstOrDefault(d => d.IdDetallePedido == detalleId);

                if (detalle != null)
                {
                    if (estado == DetalleEstado.ListoParaServir)
                        _notificationService.NotificarProductoListo(detalle.IdPedido, detalleId, detalle.NombreProducto, detalle.Cantidad);

                    _notificationService.NotificarCambioEstadoDetalle(detalle.IdPedido, detalleId, estado);
                }

                _notificationService.NotificarActualizarCocina(detalleId);

                return Json(new { success = true });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
