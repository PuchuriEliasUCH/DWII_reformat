using AltaMesa.web.Constants;
using System;
using System.Web;

namespace AltaMesa.web.Helpers
{
    public static class SessionHelper
    {
        public static int? GetUsuarioId()
        {
            var val = HttpContext.Current?.Session?[SesionConstants.UsuarioId];
            return val as int?;
        }

        public static string GetUsuarioNombre()
        {
            return HttpContext.Current?.Session?[SesionConstants.UsuarioNombre]?.ToString();
        }

        public static string GetUsuarioRol()
        {
            return HttpContext.Current?.Session?[SesionConstants.UsuarioRol]?.ToString();
        }

        public static string GetUsuarioCorreo()
        {
            return HttpContext.Current?.Session?[SesionConstants.UsuarioCorreo]?.ToString();
        }

        public static bool IsAuthenticated()
        {
            return GetUsuarioId().HasValue;
        }

        public static void SetSession(int usuarioId, string nombre, string rol, string correo)
        {
            HttpContext.Current.Session[SesionConstants.UsuarioId] = usuarioId;
            HttpContext.Current.Session[SesionConstants.UsuarioNombre] = nombre;
            HttpContext.Current.Session[SesionConstants.UsuarioRol] = rol;
            HttpContext.Current.Session[SesionConstants.UsuarioCorreo] = correo;
        }

        public static void DestroySession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();

            if (HttpContext.Current.Request.Cookies["ASP.NET_SessionId"] != null)
            {
                var cookie = new HttpCookie("ASP.NET_SessionId")
                {
                    Value = string.Empty,
                    Expires = DateTime.Now.AddMonths(-1)
                };
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }
    }
}
