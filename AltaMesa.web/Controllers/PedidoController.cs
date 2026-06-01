using AltaMesa.web.DTOs;
using AltaMesa.web.Filters;
using AltaMesa.web.Helpers;
using AltaMesa.web.Models.ViewModels;
using AltaMesa.web.Services.Interfaces;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AltaMesa.web.Controllers
{
    [AutorizarRol]
    public class PedidoController : Controller
    {
        private readonly IPedidoService _pedidoService;
        private readonly IMesaService _mesaService;
        private readonly IProductoService _productoService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public PedidoController(
            IPedidoService pedidoService,
            IMesaService mesaService,
            IProductoService productoService,
            INotificationService notificationService,
            IMapper mapper)
        {
            _pedidoService = pedidoService;
            _mesaService = mesaService;
            _productoService = productoService;
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
        public async Task<ActionResult> Crear(PedidoCrearVM model)
        {
            if (!ModelState.IsValid)
            {
                var mesasDisponibles = await _mesaService.Listar();
                model.Mesas = mesasDisponibles
                    .Where(m => m.Estado == "Disponible")
                    .Select(m => new SelectListItem
                    {
                        Value = m.IdMesa.ToString(),
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

            var productos = await _productoService.Listar();
            var vm = new PedidoDetalleVM
            {
                Pedido = pedido,
                Productos = productos
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
        public async Task<ActionResult> AgregarDetalle(PedidoDetalleVM model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Detalle", new { id = model.Pedido.IdPedido });

            await _pedidoService.AgregarDetalle(_mapper.Map<AgregarDetalleDTO>(model));

            var productos = await _productoService.Listar();
            var producto = productos.FirstOrDefault(p => p.IdProducto == model.Producto);
            var tienePreparacion = producto != null && producto.RequierePreparacion;

            if (tienePreparacion)
            {
                var pedido = await _pedidoService.ObtenerPedido(model.Pedido.IdPedido);
                var detalles = await _pedidoService.ObtenerDetalles(model.Pedido.IdPedido);
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
        public async Task<ActionResult> Cerrar(int id)
        {
            await _pedidoService.CerrarPedido(id);
            _notificationService.NotificarPedidoCerrado(id);

            TempData["Success"] = "Pedido cerrado exitosamente";
            return RedirectToAction("Index");
        }
    }
}
