using System.Linq;
using System.Web.Mvc;
using AltaMesa.web.DTOs;
using AltaMesa.web.Filters;
using AltaMesa.web.Models.ViewModels;
using AltaMesa.web.Services;

namespace AltaMesa.web.Controllers
{
    [AutorizarRol(Rol = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;
        private readonly RolService _rolService;

        public UsuarioController()
        {
            _usuarioService = new UsuarioService();
            _rolService = new RolService();
        }

        public ActionResult Index()
        {
            var vm = new UsuarioListaVM
            {
                Usuarios = _usuarioService.Listar()
            };
            return View(vm);
        }

        public ActionResult Crear()
        {
            var vm = new UsuarioCrearVM
            {
                Roles = _rolService.ListarRoles()
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
        public ActionResult Crear(UsuarioCrearVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = _rolService.ListarRoles()
                    .Select(r => new SelectListItem
                    {
                        Value = r.IdRol.ToString(),
                        Text = r.NombreRol
                    }).ToList();
                return View(model);
            }

            _usuarioService.Crear(new CrearUsuarioDTO
            {
                IdRol = model.IdRol,
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Correo = model.Correo,
                Password = model.Password
            });

            TempData["Success"] = "Usuario creado exitosamente";
            return RedirectToAction("Index");
        }

        public ActionResult Editar(int id)
        {
            var usuario = _usuarioService.Listar().FirstOrDefault(u => u.IdUsuario == id);
            if (usuario == null) return HttpNotFound();

            var vm = new UsuarioEditarVM
            {
                IdUsuario = usuario.IdUsuario,
                IdRol = usuario.IdRol,
                Nombre = usuario.NombreUsuario,
                Apellido = usuario.ApellidoUsuario,
                Correo = usuario.CorreoUsuario,
                Estado = usuario.Estado,
                Roles = _rolService.ListarRoles()
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
        public ActionResult Editar(UsuarioEditarVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = _rolService.ListarRoles()
                    .Select(r => new SelectListItem
                    {
                        Value = r.IdRol.ToString(),
                        Text = r.NombreRol
                    }).ToList();
                return View(model);
            }

            _usuarioService.Actualizar(new ActualizarUsuarioDTO
            {
                IdUsuario = model.IdUsuario,
                IdRol = model.IdRol,
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Correo = model.Correo,
                Estado = model.Estado
            });

            TempData["Success"] = "Usuario actualizado exitosamente";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            _usuarioService.Eliminar(id);
            TempData["Success"] = "Usuario desactivado exitosamente";
            return RedirectToAction("Index");
        }
    }
}
