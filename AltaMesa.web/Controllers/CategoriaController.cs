using AltaMesa.web.DTOs;
using AltaMesa.web.Filters;
using AltaMesa.web.Services.Interfaces;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AltaMesa.web.Controllers
{
    [AutorizarRol(Rol = "admin")]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;

        public CategoriaController(ICategoriaService categoriaService, IMapper mapper)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var categorias = await _categoriaService.Listar();
            return View(categorias);
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(CrearCategoriaDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _categoriaService.Crear(model);
            TempData["Success"] = "Categoría creada exitosamente";
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Editar(int id)
        {
            var categorias = await _categoriaService.Listar();
            var cat = categorias.FirstOrDefault(c => c.IdCategoria == id);
            if (cat == null) return HttpNotFound();

            return View(_mapper.Map<ActualizarCategoriaDTO>(cat));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(ActualizarCategoriaDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _categoriaService.Actualizar(model);
            TempData["Success"] = "Categoría actualizada exitosamente";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Desactivar(int id)
        {
            await _categoriaService.Desactivar(id);
            TempData["Success"] = "Categoría desactivada exitosamente";
            return RedirectToAction("Index");
        }
    }
}
