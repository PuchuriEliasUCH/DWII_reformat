using System.Data.Entity;
using AltaMesa.web.Models.Entities;

namespace AltaMesa.web.Data
{
    public class AltaMesaContext : DbContext
    {
        public AltaMesaContext() : base("name=AltaMesaDB")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<CategoriaProducto> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
        public DbSet<AuditoriaEstadoDetallePedido> AuditoriasEstado { get; set; }
    }
}
