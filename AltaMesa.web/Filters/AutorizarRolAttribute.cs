using System;
using System.Web;
using System.Web.Mvc;

namespace AltaMesa.web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AutorizarRolAttribute : AuthorizeAttribute
    {
        public string Rol { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = httpContext.Session;
            if (session == null) return false;

            var usuarioId = session["UsuarioId"];
            if (usuarioId == null) return false;

            if (string.IsNullOrEmpty(Rol)) return true;

            var usuarioRol = session["UsuarioRol"]?.ToString();
            return string.Equals(usuarioRol, Rol, StringComparison.OrdinalIgnoreCase);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UsuarioId"] == null)
            {
                filterContext.Result = new RedirectResult("~/Auth/Login");
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Error.cshtml"
                };
            }
        }
    }
}
