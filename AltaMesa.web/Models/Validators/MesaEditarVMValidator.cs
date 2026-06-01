using AltaMesa.web.Constants;
using AltaMesa.web.Models.ViewModels;
using FluentValidation;
using System.Linq;

namespace AltaMesa.web.Models.Validators
{
    public class MesaEditarVMValidator : AbstractValidator<MesaEditarVM>
    {
        private static readonly int[] CapacidadesValidas = { 2, 4, 6, 8 };
        private static readonly string[] EstadosValidos =
        {
            MesaEstado.Disponible,
            MesaEstado.Ocupada,
            MesaEstado.Inhabilitada
        };

        public MesaEditarVMValidator()
        {
            RuleFor(x => x.IdMesa)
                .GreaterThan(0);

            RuleFor(x => x.Numero)
                .GreaterThan(0);

            RuleFor(x => x.Capacidad)
                .Must(c => System.Array.IndexOf(CapacidadesValidas, c) >= 0)
                .WithMessage("La capacidad debe ser 2, 4, 6 u 8 personas");

            RuleFor(x => x.Estado)
                .NotEmpty()
                .Must(e => EstadosValidos.Contains(e))
                .WithMessage("Estado de mesa no válido");
        }
    }
}
