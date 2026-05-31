using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AltaMesa.web.Data;
using AltaMesa.web.DTOs;

namespace AltaMesa.web.Repositories
{
    public class UsuarioRepository : BaseRepository
    {
        public LoginDTO Login(string correo, string password)
        {
            using (var ctx = new AltaMesaContext())
            {
                var user = ctx.Usuarios
                    .Include("Rol")
                    .FirstOrDefault(u => u.CorreoUsuario == correo
                                     && u.ContraHash == password
                                     && u.Estado);

                if (user == null) return null;

                return new LoginDTO
                {
                    IdUsuario = user.IdUsuario,
                    NombreUsuario = user.NombreUsuario,
                    CorreoUsuario = user.CorreoUsuario,
                    NombreRol = user.Rol.NombreRol
                };
            }
        }

        public void Crear(CrearUsuarioDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_crear_usuario"))
            {
                cmd.Parameters.Add(new SqlParameter("@id_rol", dto.IdRol));
                cmd.Parameters.Add(new SqlParameter("@nombre", dto.Nombre));
                cmd.Parameters.Add(new SqlParameter("@apellido", dto.Apellido));
                cmd.Parameters.Add(new SqlParameter("@correo", dto.Correo));
                cmd.Parameters.Add(new SqlParameter("@password", dto.Password));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<UsuarioDTO> Listar()
        {
            using (var ctx = new AltaMesaContext())
            {
                return ctx.Usuarios
                    .Include("Rol")
                    .Select(u => new UsuarioDTO
                    {
                        IdUsuario = u.IdUsuario,
                        IdRol = u.IdRol,
                        NombreRol = u.Rol.NombreRol,
                        NombreUsuario = u.NombreUsuario,
                        ApellidoUsuario = u.ApellidoUsuario,
                        CorreoUsuario = u.CorreoUsuario,
                        Estado = u.Estado,
                        CreateAt = u.CreateAt
                    })
                    .ToList();
            }
        }

        public void Actualizar(ActualizarUsuarioDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_actualizar_usuario"))
            {
                cmd.Parameters.Add(new SqlParameter("@id_usuario", dto.IdUsuario));
                cmd.Parameters.Add(new SqlParameter("@id_rol", dto.IdRol));
                cmd.Parameters.Add(new SqlParameter("@nombre", dto.Nombre));
                cmd.Parameters.Add(new SqlParameter("@apellido", dto.Apellido));
                cmd.Parameters.Add(new SqlParameter("@correo", dto.Correo));
                cmd.Parameters.Add(new SqlParameter("@estado", dto.Estado));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Eliminar(int id)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_eliminar_usuario"))
            {
                cmd.Parameters.Add(new SqlParameter("@id_usuario", id));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}