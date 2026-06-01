using AltaMesa.web.Repositories;
using AltaMesa.web.Repositories.Interfaces;
using AltaMesa.web.Services;
using AltaMesa.web.Services.Interfaces;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using System.Reflection;
using System.Web.Mvc;

namespace AltaMesa.web.App_Start
{
    public class DIConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UsuarioRepository>().As<IUsuarioRepository>().InstancePerRequest();
            builder.RegisterType<MesaRepository>().As<IMesaRepository>().InstancePerRequest();
            builder.RegisterType<CategoriaRepository>().As<ICategoriaRepository>().InstancePerRequest();
            builder.RegisterType<ProductoRepository>().As<IProductoRepository>().InstancePerRequest();
            builder.RegisterType<PedidoRepository>().As<IPedidoRepository>().InstancePerRequest();
            builder.RegisterType<RolRepository>().As<IRolRepository>().InstancePerRequest();

            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerRequest();
            builder.RegisterType<UsuarioService>().As<IUsuarioService>().InstancePerRequest();
            builder.RegisterType<MesaService>().As<IMesaService>().InstancePerRequest();
            builder.RegisterType<CategoriaService>().As<ICategoriaService>().InstancePerRequest();
            builder.RegisterType<ProductoService>().As<IProductoService>().InstancePerRequest();
            builder.RegisterType<PedidoService>().As<IPedidoService>().InstancePerRequest();
            builder.RegisterType<RolService>().As<IRolService>().InstancePerRequest();
            builder.RegisterType<NotificationService>().As<INotificationService>().InstancePerRequest();

            FluentValidationConfig.RegisterValidators(builder);

            var mapperConfig = AutoMapperConfig.Register();
            builder.RegisterInstance(mapperConfig.CreateMapper()).As<IMapper>().SingleInstance();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}