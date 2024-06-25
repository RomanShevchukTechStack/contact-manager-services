using ContactManager.Data;
using ContactManager.Data.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ContactManager.Endpoints
{
  public class DeleteContactEndpoint : Endpoint<DeleteContactRequest>
  {
    private readonly Repository<Contact> _repository;

    public DeleteContactEndpoint()
    {
      _repository = new Repository<Contact>();
    }

    public override void Configure()
    {
      Delete("/api/contacts/{id:int}");
      AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteContactRequest req, CancellationToken ct)
    {
      var contactRes = _repository.GetById(req.Id);
      if (!contactRes.IsSuccess)
      {
        await SendNotFoundAsync(ct);
        return;
      }

      _repository.Delete(req.Id);
      await SendNoContentAsync(ct);
    }
  }
}
