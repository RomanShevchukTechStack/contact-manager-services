using ContactManager.Data;
using ContactManager.Data.Models;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace ContactManager.Endpoints
{
  public class CreateContactEndpoint : Endpoint<CreateContactRequest, Contact>
  {
    private readonly Repository<Contact> _repository;
    public CreateContactEndpoint()
    {
      _repository = new Repository<Contact>();
    }

    public override void Configure()
    {
      Post("/api/contacts");
      AllowAnonymous();
    }

    public override async Task HandleAsync(CreateContactRequest req, CancellationToken ct)
    {

      var contact = new Contact(req.FirstName, req.LastName, req.Email);

      var addRes = _repository.Add(contact);

      await SendCreatedAtAsync<GetContactEndpoint>(new { id = contact.Id }, addRes.Value, cancellation: ct);
    }
  }
}
