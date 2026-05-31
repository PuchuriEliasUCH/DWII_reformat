namespace AltaMesa.web.Constants
{
    public class PedidoEstado
    {
        public const string Abierto = "Abierto";
        public const string EnviadoACocina = "Enviado a cocina";
        public const string EnPreparacion = "En preparacion";
        public const string ParcialmenteServido = "Parcialmente servido";
        public const string Cerrado = "Cerrado";
        public const string Anulado = "Anulado";
    }

    public class MesaEstado
    {
        public const string Disponible = "Disponible";
        public const string Ocupada = "Ocupada";
        public const string Inhabilitada = "Inhabilitada";
    }

    public class DetalleEstado
    {
        public const string Ingresado = "Ingresado";
        public const string EnPreparacion = "En preparacion";
        public const string ListoParaServir = "Listo para servir";
        public const string Entregado = "Entregado";
        public const string Anulado = "Anulado";
    }

    public class SesionConstants
    {
        public const string UsuarioId = "UsuarioId";
        public const string UsuarioNombre = "UsuarioNombre";
        public const string UsuarioRol = "UsuarioRol";
        public const string UsuarioCorreo = "UsuarioCorreo";
    }
}
