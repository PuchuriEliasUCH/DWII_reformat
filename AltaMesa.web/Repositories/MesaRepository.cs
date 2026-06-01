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
    public class MesaRepository : BaseRepository, IMesaRepository
    {
        private readonly IMapper _mapper;

        public MesaRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task Crear(CrearMesaDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_crear_mesa"))
            {
                cmd.Parameters.Add(new SqlParameter("@numero", dto.Numero));
                cmd.Parameters.Add(new SqlParameter("@capacidad", dto.Capacidad));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task<List<MesaDTO>> Listar()
        {
            using (var ctx = new AltaMesaContext())
            {
                return await ctx.Mesas
                    .ProjectTo<MesaDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task Actualizar(ActualizarMesaDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_actualizar_mesa"))
            {
                cmd.Parameters.Add(new SqlParameter("@id", dto.Id));
                cmd.Parameters.Add(new SqlParameter("@capacidad", dto.Capacidad));
                cmd.Parameters.Add(new SqlParameter("@estado", dto.Estado));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }
    }
}