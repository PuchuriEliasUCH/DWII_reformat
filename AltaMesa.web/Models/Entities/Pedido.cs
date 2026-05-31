using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AltaMesa.web.Models.Entities
{
    [Table("pedido")]
    public class Pedido
    {
        [Key]
        [Column("id_pedido")]
        public int IdPedido { get; set; }

        [Required]
        [Column("id_mesa")]
        public int IdMesa { get; set; }

        [Required]
        [Column("id_mesero")]
        public int IdMesero { get; set; }

        [Column("fecha_pedido")]
        public DateTime FechaPedido { get; set; }

        [Column("fecha_cierre")]
        public DateTime? FechaCierre { get; set; }

        [Required]
        [MaxLength(30)]
        [Column("estado")]
        public string Estado { get; set; }

        [MaxLength(255)]
        [Column("observacion_general")]
        public string ObservacionGeneral { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        [Column("descuento")]
        public decimal Descuento { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("updated_by")]
        public int? UpdatedBy { get; set; }

        [ForeignKey("IdMesa")]
        public virtual Mesa Mesa { get; set; }

        [ForeignKey("IdMesero")]
        public virtual Usuario Mesero { get; set; }

        public virtual ICollection<DetallePedido> Detalles { get; set; }
    }
}
