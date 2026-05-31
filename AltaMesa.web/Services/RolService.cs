using System.Collections.Generic;
using System.Linq;
using AltaMesa.web.Data;
using AltaMesa.web.DTOs;

namespace AltaMesa.web.Services
{
    public class RolService
    {
        public List<RolDTO> ListarRoles()
        {
            using (var context = new AltaMesaContext())
            {
                return context.Roles
                    .Select(r => new RolDTO
                    {
                        IdRol = r.IdRol,
                        NombreRol = r.NombreRol
                    })
                    .ToList();
            }
        }
    }
}
