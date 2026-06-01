using AltaMesa.web.Models.ViewModels;
using FluentValidation;

namespace AltaMesa.web.Models.Validators
{
    public class UsuarioEditarVMValidator : AbstractValidator<UsuarioEditarVM>
    {
        public UsuarioEditarVMValidator()
        {
            RuleFor(x => x.IdUsuario)
                .GreaterThan(0);

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
        }
    }
}
