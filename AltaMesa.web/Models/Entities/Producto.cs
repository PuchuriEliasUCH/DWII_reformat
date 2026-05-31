using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AltaMesa.web.Models.Entities
{
    [Table("producto")]
    public class Producto
    {
        [Key]
        [Column("id_producto")]
        public int IdProducto { get; set; }

        [Required]
        [Column("id_categoria")]
        public int IdCategoria { get; set; }

        [Required]
        [MaxLength(80)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(80)]
        [Column("desc_corta")]
        public string DescCorta { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("desc_completa")]
        public string DescCompleta { get; set; }

        [Required]
        [Column("precio")]
        public decimal Precio { get; set; }

        [Column("requiere_preparacion")]
        public bool RequierePreparacion { get; set; }

        [Column("estado")]
        public bool Estado { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("updated_by")]
        public int? UpdatedBy { get; set; }

        [ForeignKey("IdCategoria")]
        public virtual CategoriaProducto Categoria { get; set; }
    }
}
