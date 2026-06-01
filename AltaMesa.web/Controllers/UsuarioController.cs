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
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IRolService _rolService;
        private readonly IMapper _mapper;

        public UsuarioController(
            IUsuarioService usuarioService,
            IRolService rolService,
            IMapper mapper)
        {
            _usuarioService = usuarioService;
            _rolService = rolService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var vm = new UsuarioListaVM
            {
                Usuarios = await _usuarioService.Listar()
            };
            return View(vm);
        }

        public async Task<ActionResult> Crear()
        {
            var roles = await _rolService.ListarRoles();
            var vm = new UsuarioCrearVM
            {
                Roles = roles
                    .Select(r => new SelectListItem
                    {
                        Value = r.IdRol.ToString(),
                        Text = r.NombreRol
                    }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Crear(UsuarioCrearVM model)
        {
            if (!ModelState.IsValid)
            {
                var roles = await _rolService.ListarRoles();
                model.Roles = roles
                    .Select(r => new SelectListItem
                    {
                        Value = r.IdRol.ToString(),
                        Text = r.NombreRol
                    }).ToList();
                return View(model);
            }

            await _usuarioService.Crear(_mapper.Map<CrearUsuarioDTO>(model));

            TempData["Success"] = "Usuario creado exitosamente";
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Editar(int id)
        {
            var usuarios = await _usuarioService.Listar();
            var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == id);
            if (usuario == null) return HttpNotFound();

            var roles = await _rolService.ListarRoles();
            var vm = _mapper.Map<UsuarioEditarVM>(usuario);
            vm.Roles = roles
                .Select(r => new SelectListItem
                {
                    Value = r.IdRol.ToString(),
                    Text = r.NombreRol
                }).ToList();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editar(UsuarioEditarVM model)
        {
            if (!ModelState.IsValid)
            {
                var roles = await _rolService.ListarRoles();
                model.Roles = roles
                    .Select(r => new SelectListItem
                    {
                        Value = r.IdRol.ToString(),
                        Text = r.NombreRol
                    }).ToList();
                return View(model);
            }

            await _usuarioService.Actualizar(_mapper.Map<ActualizarUsuarioDTO>(model));

            TempData["Success"] = "Usuario actualizado exitosamente";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Eliminar(int id)
        {
            await _usuarioService.Eliminar(id);
            TempData["Success"] = "Usuario desactivado exitosamente";
            return RedirectToAction("Index");
        }
    }
}
