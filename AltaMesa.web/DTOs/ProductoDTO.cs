namespace AltaMesa.web.DTOs
{
    public class ProductoDTO
    {
        public int IdProducto { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        public string Nombre { get; set; }
        public string DescCorta { get; set; }
        public string DescCompleta { get; set; }
        public decimal Precio { get; set; }
        public bool RequierePreparacion { get; set; }
        public bool Estado { get; set; }
    }

    public class CrearProductoDTO
    {
        public int Categoria { get; set; }
        public string Nombre { get; set; }
        public string Corta { get; set; }
        public string Larga { get; set; }
        public decimal Precio { get; set; }
        public bool Prep { get; set; }
    }

    public class ActualizarProductoDTO
    {
        public int IdProducto { get; set; }
        public int Categoria { get; set; }
        public string Nombre { get; set; }
        public string Corta { get; set; }
        public string Larga { get; set; }
        public decimal Precio { get; set; }
        public bool Prep { get; set; }
    }
}
