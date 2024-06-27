using ContactManager.Data;
using ContactManager.Data.Models;
using FastEndpoints;
using Serilog;

namespace ContactManager.Endpoints
{
  public class GetContactEndpoint : Endpoint<GetContactRequest, Contact>
  {
    private readonly Repository<Contact> _repository;

    public GetContactEndpoint()
    {
      _repository = new Repository<Contact>();
    }

    public override void Configure()
    {
      Get("/api/contacts/{id:int}");
      AllowAnonymous();
    }

    public override async Task HandleAsync(GetContactRequest req, CancellationToken ct)
    {
      var contactRes = _repository.GetById(req.Id);
      if (!contactRes.IsSuccess)
      {
        Log.Logger.Error($"Error: {contactRes.Errors}");
        await SendNotFoundAsync(ct);
        return;
      }

      await SendAsync(contactRes.Value, StatusCodes.Status200OK, ct);
    }
  }

}
