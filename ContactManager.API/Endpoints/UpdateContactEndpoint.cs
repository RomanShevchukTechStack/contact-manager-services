using ContactManager.Data;
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
    private readonly DataContext _context;

    public UpdateContactEndpoint()
    {
      _context = new DataContext();
    }

    public override void Configure()
    {
      Put("/api/contacts/{id:int}");
      AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateContactRequest req, CancellationToken ct)
    {
      if (!ModelState.IsValid || req.Id != Route<int>("id"))
      {
        await SendValidationErrorAsync(ct);
        return;
      }

      var contact = _context.GetContact(req.Id);
      if (contact == null)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      contact.FirstName = req.FirstName;
      contact.LastName = req.LastName;
      contact.Email = req.Email;

      _context.UpdateContact(contact);
      await SendNoContentAsync(ct);
    }
  }

  public class UpdateContactRequest
  {
    [FromRoute]
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
  }
}
