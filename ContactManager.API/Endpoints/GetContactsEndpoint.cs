using ContactManager.API.Common.Helpers;
using ContactManager.Data.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Extensions;


namespace ContactManager.Endpoints
{
  public class GetContactsEndpoint : EndpointWithoutRequest<TableContactsDTO>
  {
    private readonly Repository<Contact> _repository;

    public GetContactsEndpoint()
    {
      _repository = new Repository<Contact>();
    }

    public override void Configure()
    {
      Get("/api/contacts");
      AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
      var queryParams = QueryParamsHelper.ExtractSpecificationParams(HttpContext);
      var paginationOptions = new PaginationOptions(queryParams.PageValue, queryParams.pageSize);
      var filterOptions = new FilterOptions(queryParams.SearchValue);
      var orderOptions = new OrderOptions(queryParams.OrderBy, queryParams.OrderDirection);

      var contacts = _repository.GetAll(filterOptions, paginationOptions, orderOptions);

      var result = new TableContactsDTO(
        contacts.Value.Count(),
        queryParams.PageValue,
        (int)Math.Ceiling((double)contacts.Value.Count() / queryParams.pageSize),
        contacts.Value);

      await SendAsync(result, StatusCodes.Status200OK, ct);
    }
  }

}
