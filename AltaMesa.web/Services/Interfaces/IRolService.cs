using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AltaMesa.web.Services.Interfaces
{
    public interface IRolService
    {
        Task<List<RolDTO>> ListarRoles();
    }
}