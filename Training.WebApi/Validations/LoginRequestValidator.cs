namespace Training.WebApi.Validations;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("MSG_E001");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("MSG_E001")
            .MinimumLength(6)
            .WithMessage("MSG_E001");
    }
}