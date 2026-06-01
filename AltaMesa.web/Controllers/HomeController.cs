using AltaMesa.web.Helpers;
using System;
using System.Web.Mvc;

namespace AltaMesa.web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!SessionHelper.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var rol = SessionHelper.GetUsuarioRol();

            if (string.Equals(rol, "admin", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Index", "Dashboard");

            if (string.Equals(rol, "mesero", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Index", "Pedido");

            if (string.Equals(rol, "chef", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Index", "Cocina");

            SessionHelper.DestroySession();
            return RedirectToAction("Login", "Auth");
        }
    }
}
