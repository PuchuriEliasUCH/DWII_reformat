using AltaMesa.web.Helpers;

namespace AltaMesa.web.Services
{
    public abstract class BaseService
    {
        protected int? UsuarioId => SessionHelper.GetUsuarioId();
        protected string UsuarioNombre => SessionHelper.GetUsuarioNombre();
        protected string UsuarioRol => SessionHelper.GetUsuarioRol();
    }
}
