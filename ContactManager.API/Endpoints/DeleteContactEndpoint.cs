using ContactManager.Data;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ContactManager.Endpoints
{
  public class DeleteContactEndpoint : Endpoint<DeleteContactRequest>
  {
    private readonly DataContext _context;

    public DeleteContactEndpoint()
    {
      _context = new DataContext();
    }

    public override void Configure()
    {
      Delete("/api/contacts/{id:int}");
      AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteContactRequest req, CancellationToken ct)
    {
      var contact = _context.GetContact(req.Id);
      if (contact == null)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      _context.DeleteContact(req.Id);
      await SendNoContentAsync(ct);
    }
  }

  public class DeleteContactRequest
  {
    [FromRoute]
    public int Id { get; set; }
  }
}
