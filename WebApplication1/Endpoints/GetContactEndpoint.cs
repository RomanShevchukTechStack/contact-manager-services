using ContactManager.Data;
using ContactManager.Data.Models;
using FastEndpoints;

namespace ContactManager.Endpoints
{
  public class GetContactEndpoint : Endpoint<GetContactRequest, Contact>
  {
    private readonly DataContext _context;

    public GetContactEndpoint()
    {
      _context = new DataContext();
    }

    public override void Configure()
    {
      Get("/api/contacts/{id:int}");
      AllowAnonymous();
    }

    public override async Task HandleAsync(GetContactRequest req, CancellationToken ct)
    {
      var contact = _context.GetContact(req.Id);
      if (contact == null)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      await SendAsync(contact, StatusCodes.Status200OK, ct);
    }
  }

  public class GetContactRequest
  {
    public int Id { get; set; }
  }
}
