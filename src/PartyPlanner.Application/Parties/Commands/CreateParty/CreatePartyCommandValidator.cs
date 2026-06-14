using FluentValidation;

namespace PartyPlanner.Application.Parties.Commands.CreateParty;

public sealed class CreatePartyCommandValidator : AbstractValidator<CreatePartyCommand>
{
    public CreatePartyCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Nome da festa e obrigatorio.");
        RuleFor(x => x.ExpectedGuests).LessThanOrEqualTo(1_000_000).When(x => x.ExpectedGuests.HasValue);
        RuleFor(x => x.EstimatedBudget).GreaterThanOrEqualTo(0).LessThanOrEqualTo(999_999_999_999m).When(x => x.EstimatedBudget.HasValue);
        RuleFor(x => x.Location).MaximumLength(150).When(x => x.Location is not null);
    }
}
