using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AltaMesa.web.Models.ViewModels
{
    public class UsuarioListaVM
    {
        public List<UsuarioDTO> Usuarios { get; set; }
    }

    public class UsuarioCrearVM
    {
        [Required(ErrorMessage = "El rol es obligatorio")]
        [Display(Name = "Rol")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(50)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [MaxLength(50)]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        [MaxLength(100)]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }

    public class UsuarioEditarVM
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        [Display(Name = "Rol")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(50)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [MaxLength(50)]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        [MaxLength(100)]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Display(Name = "Activo")]
        public bool Estado { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }
}
