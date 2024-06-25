using ContactManager.Data;
using ContactManager.Data.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace ContactManager.Endpoints
{
  public class UpdateContactEndpoint : Endpoint<UpdateContactRequest>
  {
    private readonly Repository<Contact> _context;

    public UpdateContactEndpoint()
    {
      _context = new Repository<Contact>();
    }

    public override void Configure()
    {
      Put("/api/contacts/{id:int}");
      AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateContactRequest req, CancellationToken ct)
    {

      var contactRes = _context.GetById(req.Id);
      if (!contactRes.IsSuccess)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      var contact = contactRes.Value;

      contact.Update(req.FirstName, req.LastName, req.Email);

      _context.Update(contact);
      await SendNoContentAsync(ct);
    }
  }
}
