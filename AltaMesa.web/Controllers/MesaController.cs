using AltaMesa.web.Constants;
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
    [AutorizarRol(Rol = "admin, mesero")]
    public class MesaController : Controller
    {
        private readonly IMesaService _mesaService;
        private readonly IMapper _mapper;

        public MesaController(IMesaService mesaService, IMapper mapper)
        {
            _mesaService = mesaService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var vm = new MesaListaVM
            {
                Mesas = await _mesaService.Listar()
            };
            ViewBag.Vista = Session["MesaVista"]?.ToString() ?? "tabla";
            return View(vm);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerMesaJson(int id)
        {
            var mesa = await _mesaService.ObtenerPorId(id);
            if (mesa == null) return Json(null, JsonRequestBehavior.AllowGet);
            return Json(new
            {
                mesa.IdMesa,
                mesa.Numero,
                mesa.Capacidad,
                mesa.Estado
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SetVista(string vista)
        {
            Session["MesaVista"] = vista;
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> Crear(MesaCrearVM model)
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

            await _mesaService.Crear(_mapper.Map<CrearMesaDTO>(model));

            TempData["Success"] = "Mesa creada exitosamente";
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Editar(int id)
        {
            var mesa = await _mesaService.ObtenerPorId(id);
            if (mesa == null) return HttpNotFound();

            var vm = _mapper.Map<MesaEditarVM>(mesa);
            vm.Capacidades = new[] { 2, 4, 6, 8 }
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString() + " personas"
                }).ToList();
            vm.Estados = new[]
            {
                MesaEstado.Disponible,
                MesaEstado.Ocupada,
                MesaEstado.Inhabilitada
            }.Select(e => new SelectListItem
            {
                Value = e,
                Text = e
            }).ToList();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(MesaEditarVM model)
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

            await _mesaService.Actualizar(_mapper.Map<ActualizarMesaDTO>(model));

            TempData["Success"] = "Mesa actualizada exitosamente";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Inhabilitar(int id)
        {
            var mesa = await _mesaService.ObtenerPorId(id);
            if (mesa == null) return HttpNotFound();

            await _mesaService.Actualizar(new ActualizarMesaDTO
            {
                Id = mesa.IdMesa,
                Numero = mesa.Numero,
                Capacidad = mesa.Capacidad,
                Estado = MesaEstado.Inhabilitada
            });

            TempData["Success"] = "Mesa inhabilitada exitosamente";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Habilitar(int id)
        {
            var mesa = await _mesaService.ObtenerPorId(id);
            if (mesa == null) return HttpNotFound();

            await _mesaService.Actualizar(new ActualizarMesaDTO
            {
                Id = mesa.IdMesa,
                Numero = mesa.Numero,
                Capacidad = mesa.Capacidad,
                Estado = MesaEstado.Disponible
            });

            TempData["Success"] = "Mesa habilitada exitosamente";
            return RedirectToAction("Index");
        }
    }
}
