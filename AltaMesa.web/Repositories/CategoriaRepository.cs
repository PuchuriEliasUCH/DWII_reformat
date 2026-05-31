using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AltaMesa.web.Data;
using AltaMesa.web.DTOs;

namespace AltaMesa.web.Repositories
{
    public class CategoriaRepository : BaseRepository
    {
        public void Crear(CrearCategoriaDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_crear_categoria"))
            {
                cmd.Parameters.Add(new SqlParameter("@nombre", dto.Nombre));
                cmd.Parameters.Add(new SqlParameter("@descripcion", dto.Descripcion));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Actualizar(ActualizarCategoriaDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_actualizar_categoria"))
            {
                cmd.Parameters.Add(new SqlParameter("@id_categoria", dto.IdCategoria));
                cmd.Parameters.Add(new SqlParameter("@nombre", dto.Nombre));
                cmd.Parameters.Add(new SqlParameter("@descripcion", dto.Descripcion));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Desactivar(int id)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_desactivar_categoria"))
            {
                cmd.Parameters.Add(new SqlParameter("@id_categoria", id));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<CategoriaDTO> Listar()
        {
            using (var ctx = new AltaMesaContext())
            {
                return ctx.Categorias
                    .Select(c => new CategoriaDTO
                    {
                        IdCategoria = c.IdCategoria,
                        Nombre = c.Nombre,
                        Descripcion = c.Descripcion,
                        Estado = c.Estado
                    })
                    .ToList();
            }
        }
    }
}