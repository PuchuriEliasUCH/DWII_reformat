using AltaMesa.web.Helpers;
using AltaMesa.web.Models.ViewModels;
using AltaMesa.web.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AltaMesa.web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public ActionResult Login()
        {
            if (SessionHelper.IsAuthenticated())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _authService.Login(model.Correo, model.Password);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos");
                return View(model);
            }

            SessionHelper.SetSession(
                usuario.IdUsuario,
                usuario.NombreUsuario,
                usuario.NombreRol,
                usuario.CorreoUsuario
            );

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            SessionHelper.DestroySession();
            return RedirectToAction("Login");
        }
    }
}
