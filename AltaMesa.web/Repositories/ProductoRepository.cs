using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AltaMesa.web.Data;
using AltaMesa.web.DTOs;

namespace AltaMesa.web.Repositories
{
    public class ProductoRepository : BaseRepository
    {
        public void Crear(CrearProductoDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_crear_producto"))
            {
                cmd.Parameters.Add(new SqlParameter("@categoria", dto.Categoria));
                cmd.Parameters.Add(new SqlParameter("@nombre", dto.Nombre));
                cmd.Parameters.Add(new SqlParameter("@corta", dto.Corta));
                cmd.Parameters.Add(new SqlParameter("@larga", dto.Larga));
                cmd.Parameters.Add(new SqlParameter("@precio", dto.Precio));
                cmd.Parameters.Add(new SqlParameter("@prep", dto.Prep));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<ProductoDTO> Listar()
        {
            using (var ctx = new AltaMesaContext())
            {
                return ctx.Productos
                    .Include("Categoria")
                    .Select(p => new ProductoDTO
                    {
                        IdProducto = p.IdProducto,
                        IdCategoria = p.IdCategoria,
                        Categoria = p.Categoria.Nombre,
                        Nombre = p.Nombre,
                        DescCorta = p.DescCorta,
                        DescCompleta = p.DescCompleta,
                        Precio = p.Precio,
                        RequierePreparacion = p.RequierePreparacion,
                        Estado = p.Estado
                    })
                    .ToList();
            }
        }
    }
}