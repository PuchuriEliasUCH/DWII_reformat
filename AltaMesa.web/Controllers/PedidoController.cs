using AltaMesa.web.DTOs;
using AltaMesa.web.Filters;
using AltaMesa.web.Helpers;
using AltaMesa.web.Models.ViewModels;
using AltaMesa.web.Services.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AltaMesa.web.Controllers
{
    [AutorizarRol(Rol = "admin, mesero")]
    public class PedidoController : Controller
    {
        private readonly IPedidoService _pedidoService;
        private readonly IMesaService _mesaService;
        private readonly IProductoService _productoService;
        private readonly ICategoriaService _categoriaService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public PedidoController(
            IPedidoService pedidoService,
            IMesaService mesaService,
            IProductoService productoService,
            ICategoriaService categoriaService,
            INotificationService notificationService,
            IMapper mapper)
        {
            _pedidoService = pedidoService;
            _mesaService = mesaService;
            _productoService = productoService;
            _categoriaService = categoriaService;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var vm = new PedidoListaVM
            {
                Pedidos = await _pedidoService.ListarActivos()
            };
            return View(vm);
        }

        public async Task<ActionResult> Crear()
        {
            var mesas = await _mesaService.Listar();
            var vm = new PedidoCrearVM
            {
                Mesas = mesas
                    .Select(m => new SelectListItem
                    {
                        Value = m.IdMesa + "|" + m.Estado,
                        Text = "Mesa " + m.Numero + " (" + m.Capacidad + " pers.)"
                    }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(PedidoCrearVM model)
        {
            if (!ModelState.IsValid)
            {
                var listaMesas = await _mesaService.Listar();
                model.Mesas = listaMesas
                    .Select(m => new SelectListItem
                    {
                        Value = m.IdMesa + "|" + m.Estado,
                        Text = "Mesa " + m.Numero + " (" + m.Capacidad + " pers.)"
                    }).ToList();
                return View(model);
            }

            var usuarioId = SessionHelper.GetUsuarioId().GetValueOrDefault();
            var dto = _mapper.Map<CrearPedidoDTO>(model);
            dto.Mesero = usuarioId;
            var pedidoId = await _pedidoService.CrearPedido(dto);

            var mesas = await _mesaService.Listar();
            var mesa = mesas.FirstOrDefault(m => m.IdMesa == model.Mesa);
            if (mesa != null)
                _notificationService.NotificarNuevoPedido(pedidoId, mesa.Numero);

            TempData["Success"] = "Pedido creado exitosamente";
            return RedirectToAction("Detalle", new { id = pedidoId });
        }

        public async Task<ActionResult> Detalle(int id)
        {
            var pedido = await _pedidoService.ObtenerPedido(id);
            if (pedido == null) return HttpNotFound();

            var detalles = await _pedidoService.ObtenerDetalles(id);
            pedido.Detalles = detalles;

            var productos = await _productoService.Listar();
            var categorias = await _categoriaService.Listar();
            var itemsListos = await _pedidoService.ObtenerItemsListosMesero(id);

            var vm = new PedidoDetalleVM
            {
                Pedido = pedido,
                Productos = productos
                    .Where(p => p.Estado)
                    .Select(p => new SelectListItem
                    {
                        Value = p.IdProducto.ToString(),
                        Text = p.Nombre + " - $" + p.Precio
                    }).ToList(),
                ProductosConInfo = productos.Where(p => p.Estado).ToList(),
                Categorias = categorias.Where(c => c.Estado).ToList(),
                ItemsListos = _mapper.Map<List<DetalleItemVM>>(itemsListos)
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarDetalle(PedidoDetalleVM model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Detalle", new { id = model.Pedido.IdPedido });

            var usuarioId = SessionHelper.GetUsuarioId().GetValueOrDefault();
            var idDetalle = await _pedidoService.AgregarDetalle(_mapper.Map<AgregarDetalleDTO>(model), usuarioId);

            var pedido = await _pedidoService.ObtenerPedido(model.Pedido.IdPedido);
            if (pedido != null)
            {
                var detalles = await _pedidoService.ObtenerDetalles(model.Pedido.IdPedido);
                var nuevoDetalle = detalles.FirstOrDefault(d => d.IdDetallePedido == idDetalle);
                if (nuevoDetalle != null)
                {
                    if (nuevoDetalle.EstadoDetalle == "Listo para servir")
                    {
                        _notificationService.NotificarProductoListo(model.Pedido.IdPedido, idDetalle, nuevoDetalle.NombreProducto, nuevoDetalle.Cantidad);
                    }
                    else
                    {
                        _notificationService.NotificarNuevoDetalleCocina(idDetalle, pedido.NumeroMesa);
                    }
                }
                _notificationService.NotificarActualizarPedido(model.Pedido.IdPedido);
            }

            TempData["Success"] = "Producto agregado al pedido";
            return RedirectToAction("Detalle", new { id = model.Pedido.IdPedido });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarAdicional(PedidoDetalleVM model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Detalle", new { id = model.Pedido.IdPedido });

            await _pedidoService.AgregarAdicional(_mapper.Map<AgregarDetalleDTO>(model));

            var pedido = await _pedidoService.ObtenerPedido(model.Pedido.IdPedido);
            if (pedido != null)
            {
                var detalles = await _pedidoService.ObtenerDetalles(model.Pedido.IdPedido);
                var nuevoDetalle = detalles.OrderByDescending(d => d.IdDetallePedido).FirstOrDefault(d => d.IdProducto == model.Producto);
                if (nuevoDetalle != null)
                    _notificationService.NotificarNuevoDetalleCocina(nuevoDetalle.IdDetallePedido, pedido.NumeroMesa);
                _notificationService.NotificarActualizarPedido(model.Pedido.IdPedido);
            }

            TempData["Success"] = "Adicional agregado al pedido";
            return RedirectToAction("Detalle", new { id = model.Pedido.IdPedido });
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerItemsListos(int pedidoId)
        {
            var items = await _pedidoService.ObtenerItemsListosMesero(pedidoId);
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmarEntrega(int detalleId, int pedidoId)
        {
            try
            {
                var usuarioId = SessionHelper.GetUsuarioId().GetValueOrDefault();
                await _pedidoService.CambiarEstadoDetalle(detalleId, "Entregado", usuarioId);

                _notificationService.NotificarActualizarPedido(pedidoId);
                _notificationService.NotificarProductoEntregado(detalleId);

                return Json(new { success = true });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = ex.Message + " | " + ex.InnerException?.Message });
            }
        }

        public async Task<ActionResult> Historial()
        {
            var pedidos = await _pedidoService.ListarCerrados();
            return View(pedidos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cerrar(int id)
        {
            await _pedidoService.CerrarPedido(id);
            _notificationService.NotificarPedidoCerrado(id);

            TempData["Success"] = "Pedido cerrado exitosamente";
            return RedirectToAction("Index");
        }
    }
}
