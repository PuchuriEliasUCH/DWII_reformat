using System.Web.Mvc;
using AltaMesa.web.Helpers;

namespace AltaMesa.web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!SessionHelper.IsAuthenticated())
                return RedirectToAction("Login", "Auth");

            var rol = SessionHelper.GetUsuarioRol();

            if (rol == "Administrador")
                return RedirectToAction("Index", "Dashboard");

            if (rol == "Mesero")
                return RedirectToAction("Index", "Pedido");

            if (rol == "Chef")
                return RedirectToAction("Index", "Cocina");

            return RedirectToAction("Login", "Auth");
        }
    }
}
