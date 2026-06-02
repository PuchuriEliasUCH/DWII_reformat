using AltaMesa.web.DTOs;
using AltaMesa.web.Models.Entities;
using AltaMesa.web.Models.ViewModels;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace AltaMesa.web.App_Start
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration Register()
        {
            var cfg = new MapperConfigurationExpression();
            cfg.AddProfile(new MappingProfile());
            var config = new MapperConfiguration(cfg, NullLoggerFactory.Instance);
            return config;
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Entity → DTO

            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(d => d.NombreRol, o => o.MapFrom(s => s.Rol.NombreRol));

            CreateMap<Usuario, LoginDTO>()
                .ForMember(d => d.NombreRol, o => o.MapFrom(s => s.Rol.NombreRol));

            CreateMap<Rol, RolDTO>();

            CreateMap<Producto, ProductoDTO>()
                .ForMember(d => d.Categoria, o => o.MapFrom(s => s.Categoria.Nombre));

            CreateMap<Pedido, PedidoDTO>()
                .ForMember(d => d.NumeroMesa, o => o.MapFrom(s => s.Mesa.Numero))
                .ForMember(d => d.NombreMesero, o => o.MapFrom(s => s.Mesero.NombreUsuario + " " + s.Mesero.ApellidoUsuario));

            CreateMap<DetallePedido, DetallePedidoDTO>()
                .ForMember(d => d.NombreProducto, o => o.MapFrom(s => s.Producto.Nombre));

            CreateMap<DetallePedido, CocinaDTO>()
                .ForMember(d => d.NumeroMesa, o => o.MapFrom(s => s.Pedido.Mesa.Numero))
                .ForMember(d => d.NombreProducto, o => o.MapFrom(s => s.Producto.Nombre));

            CreateMap<Mesa, MesaDTO>();

            CreateMap<CategoriaProducto, CategoriaDTO>();
            CreateMap<CategoriaProducto, ActualizarCategoriaDTO>();

            #endregion

            #region DTO → ViewModel

            CreateMap<UsuarioDTO, UsuarioEditarVM>()
                .ForMember(d => d.Nombre, o => o.MapFrom(s => s.NombreUsuario))
                .ForMember(d => d.Apellido, o => o.MapFrom(s => s.ApellidoUsuario))
                .ForMember(d => d.Correo, o => o.MapFrom(s => s.CorreoUsuario));

            CreateMap<MesaDTO, MesaEditarVM>();

            CreateMap<ProductoDTO, ProductoEditarVM>()
                .ForMember(d => d.Categoria, o => o.MapFrom(s => s.IdCategoria));

            CreateMap<ProductoDTO, ProductoCrearVM>()
                .ForMember(d => d.Categoria, o => o.MapFrom(s => s.IdCategoria))
                .ForMember(d => d.Nombre, o => o.MapFrom(s => s.Nombre))
                .ForMember(d => d.Corta, o => o.MapFrom(s => s.DescCorta))
                .ForMember(d => d.Larga, o => o.MapFrom(s => s.DescCompleta))
                .ForMember(d => d.Precio, o => o.MapFrom(s => s.Precio))
                .ForMember(d => d.Prep, o => o.MapFrom(s => s.RequierePreparacion));

            CreateMap<CategoriaDTO, ActualizarCategoriaDTO>();

            #endregion

            #region ViewModel → DTO

            CreateMap<UsuarioCrearVM, CrearUsuarioDTO>()
                .ForMember(d => d.Nombre, o => o.MapFrom(s => s.Nombre))
                .ForMember(d => d.Apellido, o => o.MapFrom(s => s.Apellido))
                .ForMember(d => d.Correo, o => o.MapFrom(s => s.Correo))
                .ForMember(d => d.Password, o => o.MapFrom(s => s.Password));

            CreateMap<UsuarioEditarVM, ActualizarUsuarioDTO>()
                .ForMember(d => d.IdUsuario, o => o.MapFrom(s => s.IdUsuario))
                .ForMember(d => d.IdRol, o => o.MapFrom(s => s.IdRol))
                .ForMember(d => d.Nombre, o => o.MapFrom(s => s.Nombre))
                .ForMember(d => d.Apellido, o => o.MapFrom(s => s.Apellido))
                .ForMember(d => d.Correo, o => o.MapFrom(s => s.Correo))
                .ForMember(d => d.Estado, o => o.MapFrom(s => s.Estado));

            CreateMap<ProductoCrearVM, CrearProductoDTO>();
            CreateMap<ProductoEditarVM, ActualizarProductoDTO>();

            CreateMap<MesaCrearVM, CrearMesaDTO>();

            CreateMap<MesaEditarVM, ActualizarMesaDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.IdMesa))
                .ForMember(d => d.Numero, o => o.MapFrom(s => s.Numero));

            CreateMap<PedidoCrearVM, CrearPedidoDTO>();

            CreateMap<PedidoDetalleVM, AgregarDetalleDTO>()
                .ForMember(d => d.Pedido, o => o.MapFrom(s => s.Pedido.IdPedido));

            CreateMap<ActualizarCategoriaDTO, CategoriaProducto>();

            CreateMap<DetallePedidoDTO, DetalleItemVM>();

            #endregion
        }
    }
}
