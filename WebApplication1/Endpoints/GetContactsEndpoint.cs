using ContactManager.Data.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Http.Extensions;
using static System.Int32;

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
      var queryParams = ExtractParams();
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

    private ContactsQueryParams ExtractParams()
    {
      var url = HttpContext.Request.GetDisplayUrl();
      var uri = new Uri(url);

      var queryParameters = System.Web.HttpUtility.ParseQueryString(uri.Query);

      TryParse(queryParameters["page"]!, out var pageValue);
      TryParse(queryParameters["pageSize"]!, out var pageSize);
      var searchValue = queryParameters["search"];
      var orderBy = queryParameters["orderBy"] ?? "Id";
      var orderDirection = queryParameters["orderDirection"] ?? "asc";

      pageValue = pageValue > 0 ? pageValue : 1;
      pageSize = pageSize is > 0 and < 100 ? pageSize : 10;

      return new ContactsQueryParams(pageValue, pageSize, searchValue, orderBy, orderDirection);
    }

    public record ContactsQueryParams(int PageValue, int pageSize, string? SearchValue, string? OrderBy, string? OrderDirection);
  }
}
