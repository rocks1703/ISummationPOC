using FluentValidation;
using ISummationPOC.DBContext;
using ISummationPOC.Request;

namespace ISummationPOC.Validation
{
    public class CreateUserValidation : AbstractValidator<CreateUserRequest>
    {
        private readonly ISummationDbContext _context;

        public CreateUserValidation( ISummationDbContext dbContext)
        {
            _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            RuleFor(request => request.user).NotNull().WithMessage("User object cannot be null.");

            RuleFor(request => request.user.FirstName).NotEmpty().NotNull().WithMessage("First Name is required.");
            RuleFor(request => request.user.LastName).NotEmpty().NotNull().WithMessage("Last Name is required.");
            RuleFor(request => request.user.Email).NotEmpty().NotNull().WithMessage("Email is required.")
               .Must(BeUniqueEmail).WithMessage("Email already exists in our records.");

            RuleFor(request => request.user.Mobile).NotEmpty().NotNull().WithMessage("Mobile Number is required.")
                .Must(BeuniqueMobile).WithMessage("Mobile Number already exists in our records.")
                .Matches(@"^\+?[1-9]{1}[0-9\s\(\)\-]{6,14}$").WithMessage("Mobile number must be a valid format and not exceed 15 characters.");

        }

        private bool BeUniqueEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            return !_context.users.Any(c => c.Email == email);
        }

        private bool BeuniqueMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return false;
            return !_context.users.Any(c => c.Mobile == mobile);
        }

    }
}
