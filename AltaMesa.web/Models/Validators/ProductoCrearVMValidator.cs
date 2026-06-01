using AltaMesa.web.Models.ViewModels;
using FluentValidation;

namespace AltaMesa.web.Models.Validators
{
    public class ProductoCrearVMValidator : AbstractValidator<ProductoCrearVM>
    {
        public ProductoCrearVMValidator()
        {
            RuleFor(x => x.Categoria)
                .GreaterThan(0);

            RuleFor(x => x.Nombre)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.Corta)
                .NotEmpty()
                .MaximumLength(80);

            RuleFor(x => x.Larga)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Precio)
                .GreaterThan(0)
                .LessThan(1000000);
        }
    }
}
