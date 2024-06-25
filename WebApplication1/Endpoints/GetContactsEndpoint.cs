using ContactManager.Data;
using ContactManager.Data.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ContactManager.Endpoints
{
  public class GetContactsEndpoint : EndpointWithoutRequest<List<Contact>>
  {
    private readonly DataContext _context;

    public GetContactsEndpoint()
    {
      _context = new DataContext();
    }

    public override void Configure()
    {
      Get("/api/contacts");
      AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
      var contacts = _context.GetContacts();
      await SendAsync(contacts, StatusCodes.Status200OK, ct);
    }
  }
}
