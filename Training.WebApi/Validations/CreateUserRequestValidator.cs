using FluentValidation;
using Training.Application.Dtos.Request;

namespace Training.WebApi.Validations
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            // 👤 UserName
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("MSG_E001")                   // Bắt buộc nhập
                .MinimumLength(3).WithMessage("MSG_E003")             // Tối thiểu 3 ký tự
                .MaximumLength(50).WithMessage("MSG_E004")            // Tối đa 50 ký tự
                .Matches(@"^[A-Za-z0-9]+$").WithMessage("MSG_E007")   // Chỉ chữ và số
                .WithName("User name");

            // 📧 Mail
            RuleFor(x => x.Mail)
                .NotEmpty().WithMessage("MSG_E001")
                .MaximumLength(100).WithMessage("MSG_E004")
                .Matches(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$")
                    .WithMessage("MSG_E006")                          // Email hợp lệ
                .WithName("Email");

            // 🔐 Password
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("MSG_E001")
                .MinimumLength(8).WithMessage("MSG_E003")
                .MaximumLength(100).WithMessage("MSG_E004")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")
                    .WithMessage("MSG_E009")                          // Có ít nhất 1 hoa, 1 thường, 1 số
                .WithName("Password");

            // 🏠 Address
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("MSG_E001")
                .MaximumLength(200).WithMessage("MSG_E004")
                .WithName("Address");

            // 🎂 Age
            RuleFor(x => x.Age)
                .NotEmpty().WithMessage("MSG_E001")
                .InclusiveBetween(18, 100).WithMessage("MSG_E005")    // Trong khoảng 18–100
                .WithName("Age");

            // 🧾 FirstName
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("MSG_E001")
                .MaximumLength(50).WithMessage("MSG_E004")
                .Matches(@"^[A-Za-zÀ-ỹ\s]+$").WithMessage("MSG_E011") // Không ký tự đặc biệt
                .WithName("First name");

            // 🧾 LastName
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("MSG_E001")
                .MaximumLength(50).WithMessage("MSG_E004")
                .Matches(@"^[A-Za-zÀ-ỹ\s]+$").WithMessage("MSG_E011")
                .WithName("Last name");
        }
    }
}
