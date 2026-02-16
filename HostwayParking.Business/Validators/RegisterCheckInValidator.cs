using FluentValidation;
using HostwayParking.Communication.Request;

namespace HostwayParking.Business.Validators
{
    public class RegisterCheckInValidator : AbstractValidator<RequestRegisterCheckInJson>
    {
        public RegisterCheckInValidator()
        {
            RuleFor(x => x.Plate)
                .Must(PlateValidation.IsValidPlate).WithMessage("Placa inválida. Use o formato Brasil (ABC1D23 ou ABC1234) ou Argentina (AB123CD ou ABC123).");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("O modelo é obrigatório.")
                .MaximumLength(50).WithMessage("O modelo deve ter no máximo 50 caracteres.");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("A cor é obrigatória.")
                .MaximumLength(30).WithMessage("A cor deve ter no máximo 30 caracteres.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("O tipo é obrigatório.")
                .MaximumLength(20).WithMessage("O tipo deve ter no máximo 20 caracteres.");
        }
    }
}
