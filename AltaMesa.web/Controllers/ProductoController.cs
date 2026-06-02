using AltaMesa.web.DTOs;
using AltaMesa.web.Filters;
using AltaMesa.web.Models.ViewModels;
using AltaMesa.web.Services.Interfaces;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AltaMesa.web.Controllers
{
    [AutorizarRol(Rol = "admin")]
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;

        public ProductoController(
            IProductoService productoService,
            ICategoriaService categoriaService,
            IMapper mapper)
        {
            _productoService = productoService;
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var vm = new ProductoListaVM
            {
                Productos = await _productoService.Listar()
            };
            return View(vm);
        }

        public async Task<ActionResult> Crear()
        {
            var categorias = await _categoriaService.Listar();
            var vm = new ProductoCrearVM
            {
                Categorias = categorias
                    .Select(c => new SelectListItem
                    {
                        Value = c.IdCategoria.ToString(),
                        Text = c.Nombre
                    }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(ProductoCrearVM model)
        {
            if (!ModelState.IsValid)
            {
                var categorias = await _categoriaService.Listar();
                model.Categorias = categorias
                    .Select(c => new SelectListItem
                    {
                        Value = c.IdCategoria.ToString(),
                        Text = c.Nombre
                    }).ToList();
                return View(model);
            }

            await _productoService.Crear(_mapper.Map<CrearProductoDTO>(model));

            TempData["Success"] = "Producto creado exitosamente";
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Editar(int id)
        {
            var producto = await _productoService.ObtenerPorId(id);
            if (producto == null) return HttpNotFound();

            var categorias = await _categoriaService.Listar();
            var vm = _mapper.Map<ProductoEditarVM>(producto);
            vm.Categorias = categorias
                .Select(c => new SelectListItem
                {
                    Value = c.IdCategoria.ToString(),
                    Text = c.Nombre
                }).ToList();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(ProductoEditarVM model)
        {
            if (!ModelState.IsValid)
            {
                var categorias = await _categoriaService.Listar();
                model.Categorias = categorias
                    .Select(c => new SelectListItem
                    {
                        Value = c.IdCategoria.ToString(),
                        Text = c.Nombre
                    }).ToList();
                return View(model);
            }

            await _productoService.Actualizar(_mapper.Map<ActualizarProductoDTO>(model));

            TempData["Success"] = "Producto actualizado exitosamente";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Eliminar(int id)
        {
            await _productoService.Eliminar(id);
            TempData["Success"] = "Producto desactivado exitosamente";
            return RedirectToAction("Index");
        }
    }
}
