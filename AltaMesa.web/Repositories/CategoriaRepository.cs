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
    public class CategoriaRepository : BaseRepository, ICategoriaRepository
    {
        private readonly IMapper _mapper;

        public CategoriaRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task Crear(CrearCategoriaDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_crear_categoria"))
            {
                cmd.Parameters.Add(new SqlParameter("@nombre", dto.Nombre));
                cmd.Parameters.Add(new SqlParameter("@descripcion", dto.Descripcion));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task Actualizar(ActualizarCategoriaDTO dto)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_actualizar_categoria"))
            {
                cmd.Parameters.Add(new SqlParameter("@id_categoria", dto.IdCategoria));
                cmd.Parameters.Add(new SqlParameter("@nombre", dto.Nombre));
                cmd.Parameters.Add(new SqlParameter("@descripcion", dto.Descripcion));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task Desactivar(int id)
        {
            using (var conn = GetConnection())
            using (var cmd = GetCommand(conn, "sp_desactivar_categoria"))
            {
                cmd.Parameters.Add(new SqlParameter("@id_categoria", id));

                await conn.OpenAsync().ConfigureAwait(false);
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task<List<CategoriaDTO>> Listar()
        {
            using (var ctx = new AltaMesaContext())
            {
                return await ctx.Categorias
                    .ProjectTo<CategoriaDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}