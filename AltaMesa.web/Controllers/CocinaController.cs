using System.Linq;
using System.Web.Mvc;
using AltaMesa.web.Constants;
using AltaMesa.web.Filters;
using AltaMesa.web.Helpers;
using AltaMesa.web.Models.ViewModels;
using AltaMesa.web.Services;

namespace AltaMesa.web.Controllers
{
    [AutorizarRol(Rol = "Chef")]
    public class CocinaController : Controller
    {
        private readonly PedidoService _pedidoService;
        private readonly NotificationService _notificationService;

        public CocinaController()
        {
            _pedidoService = new PedidoService();
            _notificationService = new NotificationService();
        }

        public ActionResult Index()
        {
            var cola = _pedidoService.ListarColaCocina();
            var vm = new CocinaListaVM
            {
                ColaCocina = cola.Where(c => c.EstadoDetalle == DetalleEstado.Ingresado || c.EstadoDetalle == DetalleEstado.EnPreparacion).ToList(),
                ProductosListos = cola.Where(c => c.EstadoDetalle == DetalleEstado.ListoParaServir).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarEstado(int detalleId, string estado)
        {
            var usuarioId = SessionHelper.GetUsuarioId().GetValueOrDefault();
            _pedidoService.CambiarEstadoDetalle(detalleId, estado, usuarioId);

            if (estado == DetalleEstado.ListoParaServir)
            {
                var detalles = _pedidoService.ListarColaCocina();
                var detalle = detalles.FirstOrDefault(d => d.IdDetallePedido == detalleId);
                if (detalle != null)
                    _notificationService.NotificarProductoListo(detalle.IdPedido);
            }

            _notificationService.NotificarActualizarCocina(detalleId);

            return RedirectToAction("Index");
        }
    }
}
