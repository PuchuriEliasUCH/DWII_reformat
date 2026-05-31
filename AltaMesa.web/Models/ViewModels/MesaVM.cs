using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AltaMesa.web.Models.ViewModels
{
    public class MesaListaVM
    {
        public List<DTOs.MesaDTO> Mesas { get; set; }
    }

    public class MesaCrearVM
    {
        [Required(ErrorMessage = "El número es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "Número inválido")]
        [Display(Name = "Número de mesa")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "La capacidad es obligatoria")]
        [Display(Name = "Capacidad")]
        public int Capacidad { get; set; }

        public List<SelectListItem> Capacidades { get; set; }
    }

    public class MesaEditarVM
    {
        public int IdMesa { get; set; }

        [Display(Name = "Número")]
        public int Numero { get; set; }

        [Required(ErrorMessage = "La capacidad es obligatoria")]
        [Display(Name = "Capacidad")]
        public int Capacidad { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        public List<SelectListItem> Capacidades { get; set; }
        public List<SelectListItem> Estados { get; set; }
    }
}
