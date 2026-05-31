using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AltaMesa.web.Data;
using AltaMesa.web.DTOs;

namespace AltaMesa.web.Repositories
{
    public class PedidoRepository : BaseRepository
    {
        public int Crear(CrearPedidoDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_crear_pedido"))
            {
                cmd.Parameters.Add(new SqlParameter("@mesa", dto.Mesa));
                cmd.Parameters.Add(new SqlParameter("@mesero", dto.Mesero));
                cmd.Parameters.Add(new SqlParameter("@obs", (object)dto.Obs ?? DBNull.Value));

                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }

        public void AgregarDetalle(AgregarDetalleDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_agregar_detalle_pedido"))
            {
                cmd.Parameters.Add(new SqlParameter("@pedido", dto.Pedido));
                cmd.Parameters.Add(new SqlParameter("@producto", dto.Producto));
                cmd.Parameters.Add(new SqlParameter("@cantidad", dto.Cantidad));
                cmd.Parameters.Add(new SqlParameter("@obs", (object)dto.Obs ?? DBNull.Value));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AgregarAdicional(AgregarDetalleDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_agregar_adicional_pedido"))
            {
                cmd.Parameters.Add(new SqlParameter("@pedido", dto.Pedido));
                cmd.Parameters.Add(new SqlParameter("@producto", dto.Producto));
                cmd.Parameters.Add(new SqlParameter("@cantidad", dto.Cantidad));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Cerrar(int pedidoId)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_cerrar_pedido"))
            {
                cmd.Parameters.Add(new SqlParameter("@pedido", pedidoId));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public string ObtenerEstadoDetalle(int detalleId)
        {
            using (var ctx = new AltaMesaContext())
            {
                return ctx.DetallesPedido
                    .Where(d => d.IdDetallePedido == detalleId)
                    .Select(d => d.EstadoDetalle)
                    .FirstOrDefault();
            }
        }

        public void CambiarEstadoDetalle(int detalleId, string estado)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_cambiar_estado_detalle"))
            {
                cmd.Parameters.Add(new SqlParameter("@detalle", detalleId));
                cmd.Parameters.Add(new SqlParameter("@estado", estado));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void RegistrarAuditoria(int detalleId, string anterior, string nuevo, int usuarioId, string obs)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_registrar_auditoria_estado"))
            {
                cmd.Parameters.Add(new SqlParameter("@detalle", detalleId));
                cmd.Parameters.Add(new SqlParameter("@anterior", (object)anterior ?? DBNull.Value));
                cmd.Parameters.Add(new SqlParameter("@nuevo", nuevo));
                cmd.Parameters.Add(new SqlParameter("@usuario", usuarioId));
                cmd.Parameters.Add(new SqlParameter("@obs", (object)obs ?? DBNull.Value));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<PedidoDTO> ListarActivos()
        {
            using (var ctx = new AltaMesaContext())
            {
                return ctx.Pedidos
                    .Where(p => p.Estado != "Cerrado" && p.Estado != "Anulado")
                    .Select(p => new PedidoDTO
                    {
                        IdPedido = p.IdPedido,
                        IdMesa = p.IdMesa,
                        NumeroMesa = p.Mesa.Numero,
                        IdMesero = p.IdMesero,
                        NombreMesero = p.Mesero.NombreUsuario + " " + p.Mesero.ApellidoUsuario,
                        FechaPedido = p.FechaPedido,
                        FechaCierre = p.FechaCierre,
                        Estado = p.Estado,
                        ObservacionGeneral = p.ObservacionGeneral,
                        Subtotal = p.Subtotal,
                        Descuento = p.Descuento,
                        Total = p.Total
                    })
                    .ToList();
            }
        }

        public PedidoDTO ObtenerPorId(int id)
        {
            using (var ctx = new AltaMesaContext())
            {
                return ctx.Pedidos
                    .Where(p => p.IdPedido == id)
                    .Select(p => new PedidoDTO
                    {
                        IdPedido = p.IdPedido,
                        IdMesa = p.IdMesa,
                        NumeroMesa = p.Mesa.Numero,
                        IdMesero = p.IdMesero,
                        NombreMesero = p.Mesero.NombreUsuario + " " + p.Mesero.ApellidoUsuario,
                        FechaPedido = p.FechaPedido,
                        FechaCierre = p.FechaCierre,
                        Estado = p.Estado,
                        ObservacionGeneral = p.ObservacionGeneral,
                        Subtotal = p.Subtotal,
                        Descuento = p.Descuento,
                        Total = p.Total,
                        Detalles = p.Detalles.Select(d => new DetallePedidoDTO
                        {
                            IdDetallePedido = d.IdDetallePedido,
                            IdPedido = d.IdPedido,
                            IdProducto = d.IdProducto,
                            NombreProducto = d.Producto.Nombre,
                            Cantidad = d.Cantidad,
                            PrecioUnitario = d.PrecioUnitario,
                            Subtotal = d.Subtotal,
                            Observacion = d.Observacion,
                            EstadoDetalle = d.EstadoDetalle,
                            EsAdicional = d.EsAdicional,
                            FechaRegistro = d.FechaRegistro
                        }).ToList()
                    })
                    .FirstOrDefault();
            }
        }

        public List<DetallePedidoDTO> ObtenerDetalles(int pedidoId)
        {
            using (var ctx = new AltaMesaContext())
            {
                return ctx.DetallesPedido
                    .Where(d => d.IdPedido == pedidoId)
                    .Select(d => new DetallePedidoDTO
                    {
                        IdDetallePedido = d.IdDetallePedido,
                        IdPedido = d.IdPedido,
                        IdProducto = d.IdProducto,
                        NombreProducto = d.Producto.Nombre,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Subtotal = d.Subtotal,
                        Observacion = d.Observacion,
                        EstadoDetalle = d.EstadoDetalle,
                        EsAdicional = d.EsAdicional,
                        FechaRegistro = d.FechaRegistro
                    })
                    .ToList();
            }
        }

        public List<CocinaDTO> ListarColaCocina()
        {
            using (var ctx = new AltaMesaContext())
            {
                var estados = new[] { "Ingresado", "En preparacion", "Listo para servir" };
                return ctx.DetallesPedido
                    .Where(d => estados.Contains(d.EstadoDetalle))
                    .Select(d => new CocinaDTO
                    {
                        IdDetallePedido = d.IdDetallePedido,
                        IdPedido = d.IdPedido,
                        NombreProducto = d.Producto.Nombre,
                        NumeroMesa = d.Pedido.Mesa.Numero,
                        Cantidad = d.Cantidad,
                        Observacion = d.Observacion,
                        EstadoDetalle = d.EstadoDetalle,
                        FechaRegistro = d.FechaRegistro
                    })
                    .ToList();
            }
        }
    }
}