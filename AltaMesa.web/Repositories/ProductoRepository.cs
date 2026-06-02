using AltaMesa.web.Data;
using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AltaMesa.web.Repositories
{
    public class ProductoRepository : BaseRepository, IProductoRepository
    {
        private readonly IMapper _mapper;

        public ProductoRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task Crear(CrearProductoDTO dto)
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

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task<List<ProductoDTO>> Listar()
        {
            using (var ctx = new AltaMesaContext())
            {
                return await ctx.Productos
                    .ProjectTo<ProductoDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<ProductoDTO> ObtenerPorId(int id)
        {
            using (var ctx = new AltaMesaContext())
            {
                return await ctx.Productos
                    .Where(p => p.IdProducto == id)
                    .ProjectTo<ProductoDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task Actualizar(ActualizarProductoDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_actualizar_producto"))
            {
                cmd.Parameters.Add(new SqlParameter("@id_producto", dto.IdProducto));
                cmd.Parameters.Add(new SqlParameter("@categoria", dto.Categoria));
                cmd.Parameters.Add(new SqlParameter("@nombre", dto.Nombre));
                cmd.Parameters.Add(new SqlParameter("@corta", dto.Corta));
                cmd.Parameters.Add(new SqlParameter("@larga", dto.Larga));
                cmd.Parameters.Add(new SqlParameter("@precio", dto.Precio));
                cmd.Parameters.Add(new SqlParameter("@prep", dto.Prep));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task Eliminar(int id)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_eliminar_producto"))
            {
                cmd.Parameters.Add(new SqlParameter("@id_producto", id));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }
    }
}