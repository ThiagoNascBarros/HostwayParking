using FluentValidation;
using HostwayParking.Communication.Request;

namespace HostwayParking.Business.Validators
{
    public class RegisterCheckOutValidator : AbstractValidator<RequestRegisterCheckOutJson>
    {
        public RegisterCheckOutValidator()
        {
            RuleFor(x => x.Plate)
                .Must(PlateValidation.IsValidPlate).WithMessage("Placa inválida. Use o formato Brasil (ABC1D23 ou ABC1234) ou Argentina (AB123CD ou ABC123).");
        }
    }
}
