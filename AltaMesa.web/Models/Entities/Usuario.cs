using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AltaMesa.web.Models.Entities
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [Column("id_rol")]
        public int IdRol { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("nombre_usuario")]
        public string NombreUsuario { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("apellido_usuario")]
        public string ApellidoUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("correo_usuario")]
        public string CorreoUsuario { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("contra_hash")]
        public string ContraHash { get; set; }

        [Column("estado")]
        public bool Estado { get; set; }

        [Column("create_at")]
        public DateTime CreateAt { get; set; }

        [Column("update_at")]
        public DateTime? UpdateAt { get; set; }

        [Column("update_by")]
        public int? UpdateBy { get; set; }

        [ForeignKey("IdRol")]
        public virtual Rol Rol { get; set; }
    }
}
