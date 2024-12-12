using FluentValidation;
using ISummationPOC.DBContext;
using ISummationPOC.Entity;
using ISummationPOC.Migrations;
using ISummationPOC.Request;
using System.Reflection;
namespace ISummationPOC.Validation
{
    public class CreateUserValidation : AbstractValidator<CreateUserRequest>
    {
        private readonly ISummationDbContext _context;

        public CreateUserValidation( ISummationDbContext dbContext)
        {
            
            _context = dbContext;
            RuleFor(request => request.User).NotNull().WithMessage("User object cannot be null.");
            RuleFor(request => request.User.UserName).NotEmpty().NotNull().WithMessage("UserName is required.").Must(BeUniqueUserName).WithMessage("UserName Not Available.");
            RuleFor(request => request.User.FirstName).NotEmpty().NotNull().WithMessage("First Name is required.");
            RuleFor(request => request.User.LastName).NotEmpty().NotNull().WithMessage("Last Name is required.");

            RuleFor(request => request.User.UserDateOfBirth).NotEmpty().NotNull().Must(BeAtLeast18YearsOld).WithMessage("User must be at least 18 years old.");
            RuleFor(request => request.User.Email).NotEmpty().NotNull().WithMessage("Email is required.").Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Please provide a valid email address.").Must(BeUniqueEmail).WithMessage("Email already exists in our records.");


            RuleFor(request => request.User.Mobile).NotEmpty().NotNull().WithMessage("Mobile Number is required.").Must(BeuniqueMobile)
                .WithMessage("Mobile Number already exists in our records.")
                .Matches(@"^\+?(1[-.\s]?)?(\([2-9]\d{2}\)|[2-9]\d{2})[-.\s]?\d{3}[-.\s]?\d{4}$|^\+?91[6-9]\d{9}$")
                .WithMessage("Mobile number must be in a valid format.");

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
        private bool BeUniqueUserName(string username)
        {    
            if (string.IsNullOrWhiteSpace(username))
                return false;
            return !_context.users.Any(c => c.UserName == username);
        }

        private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age >= 18;
        }


    }
    public class UpdateUserValidation : AbstractValidator<UpdateUserRequest>
    {
        private readonly ISummationDbContext _context;
        public UpdateUserValidation(ISummationDbContext dbContext)  
        {
            _context = dbContext;

            _context = dbContext;
            RuleFor(request => request.User).NotNull().WithMessage("User object cannot be null.");
            RuleFor(request => request.User.FirstName).NotEmpty().NotNull().WithMessage("First Name is required.");

            RuleFor(request => request.User.LastName).NotEmpty().NotNull().WithMessage("Last Name is required.");
            RuleFor(request => request.User.UserDateOfBirth).NotEmpty().NotNull().Must(BeAtLeast18YearsOld).WithMessage("Customer must be at least 18 years old.");

            RuleFor(request => request.User.UserName).NotEmpty().NotNull().WithMessage("UserName is required.")
                .Must((request, username) => BeUniqueEmail(username, request.User.Id))
                .WithMessage("Email already exists in our records.");

            RuleFor(request => request.User.Email).NotEmpty().NotNull().WithMessage("Email is required.").Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Please provide a valid email address.")
               .Must((request, email) => BeUniqueEmail(email, request.User.Id))
               .WithMessage("Email already exists in our records.");

            RuleFor(request => request.User.Mobile).NotEmpty().NotNull().WithMessage("Mobile Number is required.").Must((request, mobile) 
                => BeuniqueMobile(mobile, request.User.Id ))
              .Matches(@"^\+?(1[-.\s]?)?(\([2-9]\d{2}\)|[2-9]\d{2})[-.\s]?\d{3}[-.\s]?\d{4}$|^\+?91[6-9]\d{9}$")
              .WithMessage("Mobile number must be in a valid format.");

        }
        private bool BeUniqueEmail(string email, int currentUserId)
        {
            if (string.IsNullOrWhiteSpace(email))return false;
            return !_context.users.Any(c => c.Email == email && c.Id != currentUserId);
        }
        private bool BeuniqueMobile(string mobile, int currentUserId)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            return false;
            return !_context.users.Any(c => c.Mobile == mobile && c.Id != currentUserId);
        }

        private bool BeUniqueUserName(string username, int currentUserId)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;
            return !_context.users.Any(c => c.UserName == username && c.Id != currentUserId);
        }
        private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age >= 18;
        }

    }

}
