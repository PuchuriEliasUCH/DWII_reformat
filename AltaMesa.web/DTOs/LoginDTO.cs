namespace AltaMesa.web.DTOs
{
    public class LoginDTO
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string CorreoUsuario { get; set; }
        public string NombreRol { get; set; }
    }

    public class LoginRequestDTO
    {
        public string Correo { get; set; }
        public string Password { get; set; }
    }
}
