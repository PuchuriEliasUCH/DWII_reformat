using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AltaMesa.web.Models.Entities
{
    [Table("auditoria_estado_detalle_pedido")]
    public class AuditoriaEstadoDetallePedido
    {
        [Key]
        [Column("id_auditoria")]
        public int IdAuditoria { get; set; }

        [Required]
        [Column("id_detalle_pedido")]
        public int IdDetallePedido { get; set; }

        [MaxLength(30)]
        [Column("estado_anterior")]
        public string EstadoAnterior { get; set; }

        [Required]
        [MaxLength(30)]
        [Column("estado_nuevo")]
        public string EstadoNuevo { get; set; }

        [Column("fecha_cambio")]
        public DateTime FechaCambio { get; set; }

        [Required]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [MaxLength(255)]
        [Column("observacion")]
        public string Observacion { get; set; }

        [ForeignKey("IdDetallePedido")]
        public virtual DetallePedido DetallePedido { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }
    }
}
