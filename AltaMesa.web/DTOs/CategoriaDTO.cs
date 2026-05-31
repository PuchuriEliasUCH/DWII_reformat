namespace AltaMesa.web.DTOs
{
    public class CategoriaDTO
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    }

    public class CrearCategoriaDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class ActualizarCategoriaDTO
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
