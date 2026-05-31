using System.Linq;
using System.Web.Mvc;
using AltaMesa.web.DTOs;
using AltaMesa.web.Filters;
using AltaMesa.web.Models.ViewModels;
using AltaMesa.web.Services;

namespace AltaMesa.web.Controllers
{
    [AutorizarRol(Rol = "Administrador")]
    public class CategoriaController : Controller
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController()
        {
            _categoriaService = new CategoriaService();
        }

        public ActionResult Index()
        {
            var categorias = _categoriaService.Listar();
            return View(categorias);
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(CrearCategoriaDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _categoriaService.Crear(model);
            TempData["Success"] = "Categoría creada exitosamente";
            return RedirectToAction("Index");
        }

        public ActionResult Editar(int id)
        {
            var cat = _categoriaService.Listar().FirstOrDefault(c => c.IdCategoria == id);
            if (cat == null) return HttpNotFound();

            return View(new ActualizarCategoriaDTO
            {
                IdCategoria = cat.IdCategoria,
                Nombre = cat.Nombre,
                Descripcion = cat.Descripcion
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(ActualizarCategoriaDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _categoriaService.Actualizar(model);
            TempData["Success"] = "Categoría actualizada exitosamente";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Desactivar(int id)
        {
            _categoriaService.Desactivar(id);
            TempData["Success"] = "Categoría desactivada exitosamente";
            return RedirectToAction("Index");
        }
    }
}
