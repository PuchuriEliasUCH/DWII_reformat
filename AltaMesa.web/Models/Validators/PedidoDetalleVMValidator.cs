using AltaMesa.web.Models.ViewModels;
using FluentValidation;

namespace AltaMesa.web.Models.Validators
{
    public class PedidoDetalleVMValidator : AbstractValidator<PedidoDetalleVM>
    {
        public PedidoDetalleVMValidator()
        {
            RuleFor(x => x.Producto)
                .GreaterThan(0);

            RuleFor(x => x.Cantidad)
                .GreaterThan(0);

            RuleFor(x => x.Obs)
                .MaximumLength(255);
        }
    }
}
