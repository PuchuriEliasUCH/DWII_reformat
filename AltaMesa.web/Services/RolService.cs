using AltaMesa.web.DTOs;
using AltaMesa.web.Repositories.Interfaces;
using AltaMesa.web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Services
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _rolRepository;

        public RolService(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<List<RolDTO>> ListarRoles()
        {
            return await _rolRepository.ListarRoles().ConfigureAwait(false);
        }
    }
}
