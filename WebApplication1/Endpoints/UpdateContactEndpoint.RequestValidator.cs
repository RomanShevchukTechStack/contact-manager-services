using ContactManager.Endpoints;
using FastEndpoints;
using FluentValidation;

namespace ContactManager.Validators
{
  public class UpdateContactRequestValidator : Validator<UpdateContactRequest>
  {

    public UpdateContactRequestValidator()
    {
      RuleFor(x => x.Id)
          .NotEmpty()
          .WithMessage("Id is required.");

      RuleFor(x => x.FirstName)
    .NotEmpty()
    .WithMessage("First name is required.");

      RuleFor(x => x.LastName)
          .NotEmpty()
          .WithMessage("Last name is required.");

      RuleFor(x => x.Email)
          .NotEmpty()
          .WithMessage("Email is required.")
          .EmailAddress()
          .WithMessage("Invalid email format.");

    }
  }
}
