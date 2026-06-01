using AltaMesa.web.Data;
using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AltaMesa.web.Repositories
{
    public class PedidoRepository : BaseRepository, IPedidoRepository
    {
        private readonly IMapper _mapper;

        public PedidoRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<int> Crear(CrearPedidoDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_crear_pedido"))
            {
                cmd.Parameters.Add(new SqlParameter("@mesa", dto.Mesa));
                cmd.Parameters.Add(new SqlParameter("@mesero", dto.Mesero));
                cmd.Parameters.Add(new SqlParameter("@obs", (object)dto.Obs ?? DBNull.Value));

                await conn.OpenAsync().ConfigureAwait(false);
                var result = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }

        public async Task AgregarDetalle(AgregarDetalleDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_agregar_detalle_pedido"))
            {
                cmd.Parameters.Add(new SqlParameter("@pedido", dto.Pedido));
                cmd.Parameters.Add(new SqlParameter("@producto", dto.Producto));
                cmd.Parameters.Add(new SqlParameter("@cantidad", dto.Cantidad));
                cmd.Parameters.Add(new SqlParameter("@obs", (object)dto.Obs ?? DBNull.Value));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task AgregarAdicional(AgregarDetalleDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_agregar_adicional_pedido"))
            {
                cmd.Parameters.Add(new SqlParameter("@pedido", dto.Pedido));
                cmd.Parameters.Add(new SqlParameter("@producto", dto.Producto));
                cmd.Parameters.Add(new SqlParameter("@cantidad", dto.Cantidad));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task Cerrar(int pedidoId)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_cerrar_pedido"))
            {
                cmd.Parameters.Add(new SqlParameter("@pedido", pedidoId));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task<string> ObtenerEstadoDetalle(int detalleId)
        {
            using (var ctx = new AltaMesaContext())
            {
                return await ctx.DetallesPedido
                    .Where(d => d.IdDetallePedido == detalleId)
                    .Select(d => d.EstadoDetalle)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task CambiarEstadoDetalle(int detalleId, string estado)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_cambiar_estado_detalle"))
            {
                cmd.Parameters.Add(new SqlParameter("@detalle", detalleId));
                cmd.Parameters.Add(new SqlParameter("@estado", estado));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task RegistrarAuditoria(int detalleId, string anterior, string nuevo, int usuarioId, string obs)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_registrar_auditoria_estado"))
            {
                cmd.Parameters.Add(new SqlParameter("@detalle", detalleId));
                cmd.Parameters.Add(new SqlParameter("@anterior", (object)anterior ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@nuevo", nuevo));
                cmd.Parameters.Add(new SqlParameter("@usuario", usuarioId));
                cmd.Parameters.Add(new SqlParameter("@obs", (object)obs ?? DBNull.Value));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task<List<PedidoDTO>> ListarActivos()
        {
            using (var ctx = new AltaMesaContext())
            {
                return await ctx.Pedidos
                    .Where(p => p.Estado != "Cerrado" && p.Estado != "Anulado")
                    .ProjectTo<PedidoDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<PedidoDTO> ObtenerPorId(int id)
        {
            using (var ctx = new AltaMesaContext())
            {
                return await ctx.Pedidos
                    .Where(p => p.IdPedido == id)
                    .ProjectTo<PedidoDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<List<DetallePedidoDTO>> ObtenerDetalles(int pedidoId)
        {
            using (var ctx = new AltaMesaContext())
            {
                return await ctx.DetallesPedido
                    .Where(d => d.IdPedido == pedidoId)
                    .ProjectTo<DetallePedidoDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<List<CocinaDTO>> ListarColaCocina()
        {
            using (var ctx = new AltaMesaContext())
            {
                var estados = new[] { "Ingresado", "En preparacion", "Listo para servir" };
                return await ctx.DetallesPedido
                    .Where(d => estados.Contains(d.EstadoDetalle))
                    .ProjectTo<CocinaDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}