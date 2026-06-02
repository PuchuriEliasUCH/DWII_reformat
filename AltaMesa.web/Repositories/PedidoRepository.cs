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

        public async Task<(int idDetalle, bool requierePreparacion)> AgregarDetalle(AgregarDetalleDTO dto)
        {
            using (var conn = GetConnection())
            {
                await conn.OpenAsync().ConfigureAwait(false);

                bool requierePrep;
                using (var cmdProd = new SqlCommand(
                    "SELECT requiere_preparacion FROM producto WHERE id_producto = @id", conn))
                {
                    cmdProd.Parameters.Add(new SqlParameter("@id", dto.Producto));
                    var result = await cmdProd.ExecuteScalarAsync().ConfigureAwait(false);
                    requierePrep = result != null && (bool)result;
                }

                using (var cmd = GetCommand(conn, "sp_agregar_detalle_pedido"))
                {
                    cmd.Parameters.Add(new SqlParameter("@pedido", dto.Pedido));
                    cmd.Parameters.Add(new SqlParameter("@producto", dto.Producto));
                    cmd.Parameters.Add(new SqlParameter("@cantidad", dto.Cantidad));
                    cmd.Parameters.Add(new SqlParameter("@obs", (object)dto.Obs ?? DBNull.Value));

                    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }

                int idDetalle;
                using (var cmdId = new SqlCommand(
                    "SELECT TOP 1 id_detalle_pedido FROM detalle_pedido WHERE id_pedido = @p ORDER BY fecha_registro DESC", conn))
                {
                    cmdId.Parameters.Add(new SqlParameter("@p", dto.Pedido));
                    var result = await cmdId.ExecuteScalarAsync().ConfigureAwait(false);
                    idDetalle = result != null ? Convert.ToInt32(result) : 0;
                }

                return (idDetalle, requierePrep);
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

        public async Task CambiarEstadoDetalle(int detalleId, string nuevoEstado, int usuarioId)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_cambiar_estado_detalle"))
            {
                cmd.Parameters.Add(new SqlParameter("@detalle", detalleId));
                cmd.Parameters.Add(new SqlParameter("@estado", nuevoEstado));
                cmd.Parameters.Add(new SqlParameter("@usuario", usuarioId));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task AvanzarSinPreparacion(int detalleId, int usuarioId)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_avanzar_sin_preparacion"))
            {
                cmd.Parameters.Add(new SqlParameter("@detalle", detalleId));
                cmd.Parameters.Add(new SqlParameter("@usuario", usuarioId));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task<List<DetallePedidoDTO>> ObtenerItemsListosMesero(int pedidoId)
        {
            var lista = new List<DetallePedidoDTO>();

            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(
                "SELECT id_detalle_pedido, id_pedido, id_producto, nombre_producto, " +
                "requiere_preparacion, cantidad, observacion, estado_detalle, " +
                "fecha_registro, mesa, id_mesero FROM vw_items_listos_mesero WHERE id_pedido = @pedido", conn))
            {
                cmd.Parameters.Add(new SqlParameter("@pedido", pedidoId));

                await conn.OpenAsync().ConfigureAwait(false);
                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        lista.Add(new DetallePedidoDTO
                        {
                            IdDetallePedido = (int)reader["id_detalle_pedido"],
                            IdPedido = (int)reader["id_pedido"],
                            IdProducto = (int)reader["id_producto"],
                            NombreProducto = reader["nombre_producto"].ToString(),
                            Cantidad = (int)reader["cantidad"],
                            Observacion = reader["observacion"]?.ToString(),
                            EstadoDetalle = reader["estado_detalle"].ToString(),
                        });
                    }
                }
            }
            return lista;
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

        public async Task<DashboardStatsDTO> ObtenerOrdenesDelDia()
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_dashboard_ordenes_dia"))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        return new DashboardStatsDTO
                        {
                            OrdenesDelDia = reader.GetInt32(reader.GetOrdinal("OrdenesDelDia")),
                            GananciasDelDia = reader.GetDecimal(reader.GetOrdinal("GananciasDelDia"))
                        };
                    }
                    return new DashboardStatsDTO();
                }
            }
        }

        public async Task<List<VentaDiariaDTO>> ObtenerVentasSemana()
        {
            var results = new List<VentaDiariaDTO>();
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_dashboard_ventas_semana"))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        results.Add(new VentaDiariaDTO
                        {
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                            CantidadPedidos = reader.GetInt32(reader.GetOrdinal("CantidadPedidos")),
                            TotalVentas = reader.GetDecimal(reader.GetOrdinal("TotalVentas"))
                        });
                    }
                }
            }
            return results;
        }

        public async Task<List<ProductoMasVendidoDTO>> ObtenerProductosMasVendidos(int top = 5)
        {
            var results = new List<ProductoMasVendidoDTO>();
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_productos_mas_vendidos"))
            {
                cmd.Parameters.Add(new SqlParameter("@top", top));
                await conn.OpenAsync().ConfigureAwait(false);
                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        results.Add(new ProductoMasVendidoDTO
                        {
                            IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                            NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                            TotalVendido = reader.GetInt32(reader.GetOrdinal("TotalVendido")),
                            CantidadTotal = reader.GetInt32(reader.GetOrdinal("CantidadTotal"))
                        });
                    }
                }
            }
            return results;
        }

        public async Task<List<PedidoDTO>> ListarCerrados()
        {
            var results = new List<PedidoDTO>();
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_listar_pedidos_cerrados"))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        results.Add(new PedidoDTO
                        {
                            IdPedido = reader.GetInt32(reader.GetOrdinal("id_pedido")),
                            IdMesa = reader.GetInt32(reader.GetOrdinal("id_mesa")),
                            NumeroMesa = reader.GetInt32(reader.GetOrdinal("numero_mesa")),
                            IdMesero = reader.GetInt32(reader.GetOrdinal("id_mesero")),
                            NombreMesero = reader.GetString(reader.GetOrdinal("nombre_mesero")),
                            FechaPedido = reader.GetDateTime(reader.GetOrdinal("fecha_pedido")),
                            FechaCierre = reader.IsDBNull(reader.GetOrdinal("fecha_cierre")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_cierre")),
                            Estado = reader.GetString(reader.GetOrdinal("estado")),
                            ObservacionGeneral = reader.IsDBNull(reader.GetOrdinal("observacion_general")) ? null : reader.GetString(reader.GetOrdinal("observacion_general")),
                            Subtotal = reader.GetDecimal(reader.GetOrdinal("subtotal")),
                            Descuento = reader.GetDecimal(reader.GetOrdinal("descuento")),
                            Total = reader.GetDecimal(reader.GetOrdinal("total"))
                        });
                    }
                }
            }
            return results;
        }

        public async Task<List<CocinaDTO>> ListarColaCocina()
        {
            using (var ctx = new AltaMesaContext())
            {
                var estados = new[] { "Ingresado", "En preparacion", "Listo para servir" };
                return await ctx.DetallesPedido
                    .Where(d => estados.Contains(d.EstadoDetalle) && d.Producto.RequierePreparacion)
                    .ProjectTo<CocinaDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}
