using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AltaMesa.web.Models.Entities
{
    [Table("categoria_producto")]
    public class CategoriaProducto
    {
        [Key]
        [Column("id_categoria")]
        public int IdCategoria { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [MaxLength(150)]
        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("estado")]
        public bool Estado { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
