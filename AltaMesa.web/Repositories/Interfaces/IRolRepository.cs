using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Repositories.Interfaces
{
    public interface IRolRepository
    {
        Task<List<RolDTO>> ListarRoles();
    }
}