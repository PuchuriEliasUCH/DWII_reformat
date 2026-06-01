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
    public class UsuarioRepository : BaseRepository, IUsuarioRepository
    {
        private readonly IMapper _mapper;

        public UsuarioRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<LoginDTO> ObtenerPorCorreo(string correo)
        {
            using (var ctx = new AltaMesaContext())
            {
                var user = await ctx.Usuarios
                    .Include("Rol")
                    .FirstOrDefaultAsync(u => u.CorreoUsuario == correo
                                           && u.Estado)
                    .ConfigureAwait(false);

                if (user == null) return null;

                return new LoginDTO
                {
                    IdUsuario = user.IdUsuario,
                    NombreUsuario = user.NombreUsuario,
                    CorreoUsuario = user.CorreoUsuario,
                    NombreRol = user.Rol.NombreRol,
                    ContraHash = user.ContraHash
                };
            }
        }

        public async Task Crear(CrearUsuarioDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_crear_usuario"))
            {
                cmd.Parameters.Add(new SqlParameter("@id_rol", dto.IdRol));
                cmd.Parameters.Add(new SqlParameter("@nombre", dto.Nombre));
                cmd.Parameters.Add(new SqlParameter("@apellido", dto.Apellido));
                cmd.Parameters.Add(new SqlParameter("@correo", dto.Correo));
                cmd.Parameters.Add(new SqlParameter("@password", dto.Password));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task<List<UsuarioDTO>> Listar()
        {
            using (var ctx = new AltaMesaContext())
            {
                return await ctx.Usuarios
                    .ProjectTo<UsuarioDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task Actualizar(ActualizarUsuarioDTO dto)
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

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task Eliminar(int id)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_eliminar_usuario"))
            {
                cmd.Parameters.Add(new SqlParameter("@id_usuario", id));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }
    }
}