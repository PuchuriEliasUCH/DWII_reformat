using System.Collections.Generic;
using System.Linq;
using AltaMesa.web.Data;
using AltaMesa.web.DTOs;

namespace AltaMesa.web.Repositories
{
    public class RolRepository
    {
        public List<RolDTO> ListarRoles()
        {
            using (var ctx = new AltaMesaContext())
            {
                return ctx.Roles
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