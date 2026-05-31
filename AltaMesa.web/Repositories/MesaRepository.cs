using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AltaMesa.web.Data;
using AltaMesa.web.DTOs;

namespace AltaMesa.web.Repositories
{
    public class MesaRepository : BaseRepository
    {
        public void Crear(CrearMesaDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_crear_mesa"))
            {
                cmd.Parameters.Add(new SqlParameter("@numero", dto.Numero));
                cmd.Parameters.Add(new SqlParameter("@capacidad", dto.Capacidad));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<MesaDTO> Listar()
        {
            using (var ctx = new AltaMesaContext())
            {
                return ctx.Mesas
                    .Select(m => new MesaDTO
                    {
                        IdMesa = m.IdMesa,
                        Numero = m.Numero,
                        Capacidad = m.Capacidad,
                        Estado = m.Estado
                    })
                    .ToList();
            }
        }

        public void Actualizar(ActualizarMesaDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_actualizar_mesa"))
            {
                cmd.Parameters.Add(new SqlParameter("@id", dto.Id));
                cmd.Parameters.Add(new SqlParameter("@capacidad", dto.Capacidad));
                cmd.Parameters.Add(new SqlParameter("@estado", dto.Estado));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}