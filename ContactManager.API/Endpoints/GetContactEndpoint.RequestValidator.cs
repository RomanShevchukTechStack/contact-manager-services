using ContactManager.Endpoints;
using FastEndpoints;
using FluentValidation;

namespace ContactManager.Validators
{
  public class GetContactRequestValidator : Validator<GetContactRequest>
  {

    public GetContactRequestValidator()
    {
      RuleFor(x => x.Id)
          .NotEmpty()
          .WithMessage("Id is required.");

    }
  }
}
