using Core.Filters.Login;
using FluentValidation;

namespace Service.Validators
{
    public class LoginValidatorService : AbstractValidator<UserLoginFilter>
    {
        public LoginValidatorService()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .NotNull()
                .MaximumLength(200)
                .EmailAddress()
                .MinimumLength(6);

            RuleFor(user => user.Password)
                .NotEmpty()
                .NotNull()
                .MaximumLength(200)
                .MinimumLength(6);
                

        }    
    }
}
