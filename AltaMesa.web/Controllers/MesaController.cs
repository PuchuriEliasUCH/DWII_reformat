using System.Linq;
using System.Web.Mvc;
using AltaMesa.web.Constants;
using AltaMesa.web.DTOs;
using AltaMesa.web.Filters;
using AltaMesa.web.Models.ViewModels;
using AltaMesa.web.Services;

namespace AltaMesa.web.Controllers
{
    [AutorizarRol]
    public class MesaController : Controller
    {
        private readonly MesaService _mesaService;

        public MesaController()
        {
            _mesaService = new MesaService();
        }

        public ActionResult Index()
        {
            var vm = new MesaListaVM
            {
                Mesas = _mesaService.Listar()
            };
            return View(vm);
        }

        public ActionResult Crear()
        {
            var vm = new MesaCrearVM
            {
                Capacidades = new[] { 2, 4, 6, 8 }
                    .Select(c => new SelectListItem
                    {
                        Value = c.ToString(),
                        Text = c.ToString() + " personas"
                    }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(MesaCrearVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Capacidades = new[] { 2, 4, 6, 8 }
                    .Select(c => new SelectListItem
                    {
                        Value = c.ToString(),
                        Text = c.ToString() + " personas"
                    }).ToList();
                return View(model);
            }

            _mesaService.Crear(new CrearMesaDTO
            {
                Numero = model.Numero,
                Capacidad = model.Capacidad
            });

            TempData["Success"] = "Mesa creada exitosamente";
            return RedirectToAction("Index");
        }

        public ActionResult Editar(int id)
        {
            var mesa = _mesaService.Listar().FirstOrDefault(m => m.IdMesa == id);
            if (mesa == null) return HttpNotFound();

            var vm = new MesaEditarVM
            {
                IdMesa = mesa.IdMesa,
                Numero = mesa.Numero,
                Capacidad = mesa.Capacidad,
                Estado = mesa.Estado,
                Capacidades = new[] { 2, 4, 6, 8 }
                    .Select(c => new SelectListItem
                    {
                        Value = c.ToString(),
                        Text = c.ToString() + " personas"
                    }).ToList(),
                Estados = new[]
                {
                    MesaEstado.Disponible,
                    MesaEstado.Ocupada,
                    MesaEstado.Inhabilitada
                }.Select(e => new SelectListItem
                {
                    Value = e,
                    Text = e
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(MesaEditarVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Capacidades = new[] { 2, 4, 6, 8 }
                    .Select(c => new SelectListItem
                    {
                        Value = c.ToString(),
                        Text = c.ToString() + " personas"
                    }).ToList();
                model.Estados = new[]
                {
                    MesaEstado.Disponible,
                    MesaEstado.Ocupada,
                    MesaEstado.Inhabilitada
                }.Select(e => new SelectListItem
                {
                    Value = e,
                    Text = e
                }).ToList();
                return View(model);
            }

            _mesaService.Actualizar(new ActualizarMesaDTO
            {
                Id = model.IdMesa,
                Capacidad = model.Capacidad,
                Estado = model.Estado
            });

            TempData["Success"] = "Mesa actualizada exitosamente";
            return RedirectToAction("Index");
        }
    }
}
