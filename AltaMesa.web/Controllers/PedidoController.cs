using System.Linq;
using System.Web.Mvc;
using AltaMesa.web.DTOs;
using AltaMesa.web.Filters;
using AltaMesa.web.Helpers;
using AltaMesa.web.Models.ViewModels;
using AltaMesa.web.Services;

namespace AltaMesa.web.Controllers
{
    [AutorizarRol]
    public class PedidoController : Controller
    {
        private readonly PedidoService _pedidoService;
        private readonly MesaService _mesaService;
        private readonly ProductoService _productoService;
        private readonly NotificationService _notificationService;

        public PedidoController()
        {
            _pedidoService = new PedidoService();
            _mesaService = new MesaService();
            _productoService = new ProductoService();
            _notificationService = new NotificationService();
        }

        public ActionResult Index()
        {
            var vm = new PedidoListaVM
            {
                Pedidos = _pedidoService.ListarActivos()
            };
            return View(vm);
        }

        public ActionResult Crear()
        {
            var vm = new PedidoCrearVM
            {
                Mesas = _mesaService.Listar()
                    .Where(m => m.Estado == "Disponible")
                    .Select(m => new SelectListItem
                    {
                        Value = m.IdMesa.ToString(),
                        Text = "Mesa " + m.Numero + " (" + m.Capacidad + " pers.)"
                    }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(PedidoCrearVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Mesas = _mesaService.Listar()
                    .Where(m => m.Estado == "Disponible")
                    .Select(m => new SelectListItem
                    {
                        Value = m.IdMesa.ToString(),
                        Text = "Mesa " + m.Numero + " (" + m.Capacidad + " pers.)"
                    }).ToList();
                return View(model);
            }

            var usuarioId = SessionHelper.GetUsuarioId().GetValueOrDefault();
            var pedidoId = _pedidoService.CrearPedido(new CrearPedidoDTO
            {
                Mesa = model.Mesa,
                Mesero = usuarioId,
                Obs = model.Obs
            });

            var mesa = _mesaService.Listar().FirstOrDefault(m => m.IdMesa == model.Mesa);
            if (mesa != null)
                _notificationService.NotificarNuevoPedido(pedidoId, mesa.Numero);

            TempData["Success"] = "Pedido creado exitosamente";
            return RedirectToAction("Detalle", new { id = pedidoId });
        }

        public ActionResult Detalle(int id)
        {
            var pedido = _pedidoService.ObtenerPedido(id);
            if (pedido == null) return HttpNotFound();

            var vm = new PedidoDetalleVM
            {
                Pedido = pedido,
                Productos = _productoService.Listar()
                    .Where(p => p.Estado)
                    .Select(p => new SelectListItem
                    {
                        Value = p.IdProducto.ToString(),
                        Text = p.Nombre + " - $" + p.Precio
                    }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarDetalle(PedidoDetalleVM model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Detalle", new { id = model.Pedido.IdPedido });

            _pedidoService.AgregarDetalle(new AgregarDetalleDTO
            {
                Pedido = model.Pedido.IdPedido,
                Producto = model.Producto,
                Cantidad = model.Cantidad,
                Obs = model.Obs
            });

            var producto = _productoService.Listar().FirstOrDefault(p => p.IdProducto == model.Producto);
            var tienePreparacion = producto != null && producto.RequierePreparacion;

            if (tienePreparacion)
            {
                var pedido = _pedidoService.ObtenerPedido(model.Pedido.IdPedido);
                var detalles = _pedidoService.ObtenerDetalles(model.Pedido.IdPedido);
                var nuevoDetalle = detalles.FirstOrDefault(d => d.IdProducto == model.Producto);
                if (nuevoDetalle != null && pedido != null)
                    _notificationService.NotificarNuevoDetalleCocina(nuevoDetalle.IdDetallePedido, pedido.NumeroMesa);
            }

            _notificationService.NotificarActualizarPedido(model.Pedido.IdPedido);

            TempData["Success"] = "Producto agregado al pedido";
            return RedirectToAction("Detalle", new { id = model.Pedido.IdPedido });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cerrar(int id)
        {
            _pedidoService.CerrarPedido(id);
            _notificationService.NotificarPedidoCerrado(id);

            TempData["Success"] = "Pedido cerrado exitosamente";
            return RedirectToAction("Index");
        }
    }
}
