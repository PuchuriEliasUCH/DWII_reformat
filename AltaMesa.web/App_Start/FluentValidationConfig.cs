using AltaMesa.web.Filters;
using AltaMesa.web.Models.Validators;
using Autofac;
using FluentValidation;

namespace AltaMesa.web.App_Start
{
    public class FluentValidationConfig
    {
        public static void RegisterValidators(ContainerBuilder builder)
        {
            builder.RegisterType<LoginVMValidator>().As<IValidator<Models.ViewModels.LoginVM>>().InstancePerRequest();
            builder.RegisterType<UsuarioCrearVMValidator>().As<IValidator<Models.ViewModels.UsuarioCrearVM>>().InstancePerRequest();
            builder.RegisterType<UsuarioEditarVMValidator>().As<IValidator<Models.ViewModels.UsuarioEditarVM>>().InstancePerRequest();
            builder.RegisterType<MesaCrearVMValidator>().As<IValidator<Models.ViewModels.MesaCrearVM>>().InstancePerRequest();
            builder.RegisterType<MesaEditarVMValidator>().As<IValidator<Models.ViewModels.MesaEditarVM>>().InstancePerRequest();
            builder.RegisterType<ProductoCrearVMValidator>().As<IValidator<Models.ViewModels.ProductoCrearVM>>().InstancePerRequest();
            builder.RegisterType<PedidoCrearVMValidator>().As<IValidator<Models.ViewModels.PedidoCrearVM>>().InstancePerRequest();
            builder.RegisterType<PedidoDetalleVMValidator>().As<IValidator<Models.ViewModels.PedidoDetalleVM>>().InstancePerRequest();
        }

        public static void RegisterFilter(System.Web.Mvc.GlobalFilterCollection filters)
        {
            filters.Add(new FluentValidationFilter());
        }
    }
}
