using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AltaMesa.web.Models.Entities
{
    [Table("mesa")]
    public class Mesa
    {
        [Key]
        [Column("id_mesa")]
        public int IdMesa { get; set; }

        [Required]
        [Column("numero")]
        public int Numero { get; set; }

        [Required]
        [Column("capacidad")]
        public int Capacidad { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("estado")]
        public string Estado { get; set; }
    }
}
