using FluentValidation;
using WebApplication.Models.Database;

public class TicketOrderValidator : AbstractValidator<TicketOrder> {
    public TicketOrderValidator() {
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Phone).NotEmpty();
        RuleFor(x => x.TicketType).NotEmpty();
        RuleFor(x => x.ZipCode).Length(5, 6);
        RuleFor(x => x.Tickets).NotEmpty();
    }
}