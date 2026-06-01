using AltaMesa.web.Data;
using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AltaMesa.web.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly IMapper _mapper;

        public RolRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<RolDTO>> ListarRoles()
        {
            using (var ctx = new AltaMesaContext())
            {
                return await ctx.Roles
                    .ProjectTo<RolDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}