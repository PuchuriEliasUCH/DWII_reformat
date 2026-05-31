using System.Linq;
using System.Web.Mvc;
using AltaMesa.web.DTOs;
using AltaMesa.web.Filters;
using AltaMesa.web.Models.ViewModels;
using AltaMesa.web.Services;

namespace AltaMesa.web.Controllers
{
    [AutorizarRol(Rol = "Administrador")]
    public class ProductoController : Controller
    {
        private readonly ProductoService _productoService;
        private readonly CategoriaService _categoriaService;

        public ProductoController()
        {
            _productoService = new ProductoService();
            _categoriaService = new CategoriaService();
        }

        public ActionResult Index()
        {
            var vm = new ProductoListaVM
            {
                Productos = _productoService.Listar()
            };
            return View(vm);
        }

        public ActionResult Crear()
        {
            var vm = new ProductoCrearVM
            {
                Categorias = _categoriaService.Listar()
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
        public ActionResult Crear(ProductoCrearVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Categorias = _categoriaService.Listar()
                    .Select(c => new SelectListItem
                    {
                        Value = c.IdCategoria.ToString(),
                        Text = c.Nombre
                    }).ToList();
                return View(model);
            }

            _productoService.Crear(new CrearProductoDTO
            {
                Categoria = model.Categoria,
                Nombre = model.Nombre,
                Corta = model.Corta,
                Larga = model.Larga,
                Precio = model.Precio,
                Prep = model.Prep
            });

            TempData["Success"] = "Producto creado exitosamente";
            return RedirectToAction("Index");
        }
    }
}
