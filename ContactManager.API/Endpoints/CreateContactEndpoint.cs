using ContactManager.Data;
using ContactManager.Data.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace ContactManager.Endpoints
{
  public class CreateContactEndpoint : Endpoint<CreateContactRequest, Contact>
  {
    private readonly DataContext _context;

    public CreateContactEndpoint()
    {
      _context = new DataContext();
    }

    public override void Configure()
    {
      Post("/api/contacts");
      AllowAnonymous();
    }

    public override async Task HandleAsync(CreateContactRequest req, CancellationToken ct)
    {
      if (!ModelState.IsValid)
      {
        await SendValidationErrorAsync(ct);
        return;
      }

      var contact = new Contact
      {
        FirstName = req.FirstName,
        LastName = req.LastName,
        Email = req.Email
      };

      _context.AddContact(contact);
      await SendCreatedAtAsync<GetContactEndpoint>(new { id = contact.Id }, contact, cancellation: ct);
    }
  }

  public class CreateContactRequest
  {
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
  }
}
