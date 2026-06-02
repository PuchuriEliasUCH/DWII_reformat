using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AltaMesa.web.Models.ViewModels
{
    public class ProductoListaVM
    {
        public List<ProductoDTO> Productos { get; set; }
    }

    public class ProductoCrearVM
    {
        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int Categoria { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(80)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción corta es obligatoria")]
        [MaxLength(80)]
        [Display(Name = "Descripción corta")]
        public string Corta { get; set; }

        [Required(ErrorMessage = "La descripción completa es obligatoria")]
        [MaxLength(255)]
        [Display(Name = "Descripción completa")]
        public string Larga { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, 999999.99, ErrorMessage = "Precio inválido")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Display(Name = "Requiere preparación")]
        public bool Prep { get; set; }

        public List<SelectListItem> Categorias { get; set; }
    }

    public class ProductoEditarVM
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int Categoria { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(80)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción corta es obligatoria")]
        [MaxLength(80)]
        [Display(Name = "Descripción corta")]
        public string Corta { get; set; }

        [Required(ErrorMessage = "La descripción completa es obligatoria")]
        [MaxLength(255)]
        [Display(Name = "Descripción completa")]
        public string Larga { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, 999999.99, ErrorMessage = "Precio inválido")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        [Display(Name = "Requiere preparación")]
        public bool Prep { get; set; }

        public List<SelectListItem> Categorias { get; set; }
    }
}
