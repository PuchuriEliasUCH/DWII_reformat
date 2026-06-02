using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AltaMesa.web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AutorizarRolAttribute : AuthorizeAttribute
    {
        private string _roles;
        public string Rol
        {
            get => _roles;
            set => _roles = value;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = httpContext.Session;
            if (session == null) return false;

            var usuarioId = session["UsuarioId"];
            if (usuarioId == null) return false;

            if (string.IsNullOrEmpty(Rol)) return true;

            var roles = Rol.Split(',').Select(r => r.Trim());
            var usuarioRol = session["UsuarioRol"]?.ToString();
            return roles.Any(r => string.Equals(usuarioRol, r, StringComparison.OrdinalIgnoreCase));
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
