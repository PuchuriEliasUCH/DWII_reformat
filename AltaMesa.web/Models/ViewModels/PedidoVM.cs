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
        public int Cantidad { get; set; } = 1;

        [Display(Name = "Observación")]
        [MaxLength(255)]
        public string Obs { get; set; }

        public List<SelectListItem> Productos { get; set; }
        public List<ProductoDTO> ProductosConInfo { get; set; }
        public List<CategoriaDTO> Categorias { get; set; }
        public List<DetalleItemVM> ItemsListos { get; set; } = new List<DetalleItemVM>();
    }

    public class CategoriaIndexVM
    {
        public List<CategoriaDTO> Categorias { get; set; }
        public Dictionary<int, int> ConteoProductos { get; set; }
    }

    public class CocinaListaVM
    {
        public List<CocinaDTO> NuevosItems { get; set; }
        public List<CocinaDTO> EnPreparacion { get; set; }
        public List<CocinaDTO> ProductosListos { get; set; }
    }

    public class CocinaItemVM
    {
        public int IdDetallePedido { get; set; }
        public int IdPedido { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public int Mesa { get; set; }
        public string Observacion { get; set; }
        public string EstadoDetalle { get; set; }
    }

    public class DetalleItemVM
    {
        public int IdDetallePedido { get; set; }
        public int IdPedido { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public string Observacion { get; set; }
        public string EstadoDetalle { get; set; }
        public bool RequierePreparacion { get; set; }
    }

    public class DashboardVM
    {
        public int MesasOcupadas { get; set; }
        public int MesasDisponibles { get; set; }
        public int PedidosActivos { get; set; }
        public int PlatosEnPreparacion { get; set; }
        public int PlatosListos { get; set; }
        public int OrdenesDelDia { get; set; }
        public decimal GananciasDelDia { get; set; }
        public List<PedidoDTO> UltimosPedidos { get; set; }
        public List<VentaDiariaDTO> VentasSemana { get; set; }
        public List<ProductoMasVendidoDTO> ProductosMasVendidos { get; set; }
    }
}
