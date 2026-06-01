using AltaMesa.web.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AltaMesa.web.Models.ViewModels
{
    public class PedidoListaVM
    {
        public List<PedidoDTO> Pedidos { get; set; }
    }

    public class PedidoCrearVM
    {
        [Required(ErrorMessage = "La mesa es obligatoria")]
        [Display(Name = "Mesa")]
        public int Mesa { get; set; }

        [Display(Name = "Observación")]
        [MaxLength(255)]
        public string Obs { get; set; }

        public List<SelectListItem> Mesas { get; set; }
    }

    public class PedidoDetalleVM
    {
        public PedidoDTO Pedido { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [Display(Name = "Producto")]
        public int Producto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Cantidad inválida")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Display(Name = "Observación")]
        [MaxLength(255)]
        public string Obs { get; set; }

        public List<SelectListItem> Productos { get; set; }
    }

    public class CocinaListaVM
    {
        public List<CocinaDTO> ColaCocina { get; set; }
        public List<CocinaDTO> ProductosListos { get; set; }
    }

    public class DashboardVM
    {
        public int MesasOcupadas { get; set; }
        public int MesasDisponibles { get; set; }
        public int PedidosActivos { get; set; }
        public int ProductosPendientes { get; set; }
        public List<PedidoDTO> UltimosPedidos { get; set; }
    }
}
