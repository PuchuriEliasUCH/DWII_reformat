using AltaMesa.web.Models.ViewModels;
using FluentValidation;

namespace AltaMesa.web.Models.Validators
{
    public class UsuarioCrearVMValidator : AbstractValidator<UsuarioCrearVM>
    {
        public UsuarioCrearVMValidator()
        {
            RuleFor(x => x.IdRol)
                .GreaterThan(0);

            RuleFor(x => x.Nombre)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Apellido)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Correo)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(100);
        }
    }
}
