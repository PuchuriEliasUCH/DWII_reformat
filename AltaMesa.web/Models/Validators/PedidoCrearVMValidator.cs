using AltaMesa.web.Models.ViewModels;
using FluentValidation;

namespace AltaMesa.web.Models.Validators
{
    public class PedidoCrearVMValidator : AbstractValidator<PedidoCrearVM>
    {
        public PedidoCrearVMValidator()
        {
            RuleFor(x => x.Mesa)
                .GreaterThan(0);

            RuleFor(x => x.Obs)
                .MaximumLength(255);
        }
    }
}
