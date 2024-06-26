using ContactManager.Endpoints;
using FastEndpoints;
using FluentValidation;

namespace ContactManager.Validators
{
  public class DeleteContactRequestValidator : Validator<DeleteContactRequest>
  {
    public DeleteContactRequestValidator()
    {
      RuleFor(x => x.Id)
          .NotEmpty()
          .WithMessage("Id is required.");
    }
  }
}
