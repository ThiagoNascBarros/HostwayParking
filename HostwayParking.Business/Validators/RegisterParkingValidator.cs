using FluentValidation;
using HostwayParking.Communication.Request;

namespace HostwayParking.Business.Validators
{
    public class RegisterParkingValidator : AbstractValidator<RequestRegisterParkingJson>
    {
        public RegisterParkingValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("O código do pátio é obrigatório.")
                .MaximumLength(12).WithMessage("O código deve ter no máximo 12 caracteres.");

            RuleFor(x => x.Address).NotNull().WithMessage("O endereço é obrigatório.");

            When(x => x.Address != null, () =>
            {
                RuleFor(x => x.Address.State)
                    .NotEmpty().WithMessage("O estado é obrigatório.");

                RuleFor(x => x.Address.City)
                    .NotEmpty().WithMessage("A cidade é obrigatória.");

                RuleFor(x => x.Address.Number)
                    .NotEmpty().WithMessage("O número é obrigatório.");
            });
        }
    }
}
