using AltaMesa.web.Models.ViewModels;
using FluentValidation;

namespace AltaMesa.web.Models.Validators
{
    public class MesaCrearVMValidator : AbstractValidator<MesaCrearVM>
    {
        private static readonly int[] CapacidadesValidas = { 2, 4, 6, 8 };

        public MesaCrearVMValidator()
        {
            RuleFor(x => x.Numero)
                .GreaterThan(0);

            RuleFor(x => x.Capacidad)
                .Must(c => System.Array.IndexOf(CapacidadesValidas, c) >= 0)
                .WithMessage("La capacidad debe ser 2, 4, 6 u 8 personas");
        }
    }
}
