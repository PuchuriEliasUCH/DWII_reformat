namespace AltaMesa.web.DTOs
{
    public class MesaDTO
    {
        public int IdMesa { get; set; }
        public int Numero { get; set; }
        public int Capacidad { get; set; }
        public string Estado { get; set; }
    }

    public class CrearMesaDTO
    {
        public int Numero { get; set; }
        public int Capacidad { get; set; }
    }

    public class ActualizarMesaDTO
    {
        public int Id { get; set; }
        public int Capacidad { get; set; }
        public string Estado { get; set; }
    }
}
