using FluentValidation;

namespace Gamestore.BLL.DTOs.Order;

public class OrderHistoryOptionsValidator : AbstractValidator<OrderHistoryOptions>
{
    public OrderHistoryOptionsValidator()
    {
        RuleFor(oh => oh.End)
            .GreaterThan(oh => oh.Start)
            .WithMessage("End date must be greater than start date.");
    }
}
