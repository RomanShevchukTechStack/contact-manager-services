using Microsoft.AspNetCore.Http.Extensions;
using System.Web;
using static System.Int32;
namespace ContactManager.API.Common.Helpers
{
  public static class QueryParamsHelper
  {
    public static QueryParams ExtractSpecificationParams(HttpContext httpContext)
    {
      var url = httpContext.Request.GetDisplayUrl();
      var uri = new Uri(url);

      var queryParameters = HttpUtility.ParseQueryString(uri.Query);

      TryParse(queryParameters["page"]!, out var pageValue);
      TryParse(queryParameters["pageSize"]!, out var pageSize);
      var searchValue = queryParameters["search"];
      var orderBy = queryParameters["orderBy"] ?? "Id";
      var orderDirection = queryParameters["orderDirection"] ?? "asc";

      pageValue = pageValue > 0 ? pageValue : 1;
      pageSize = pageSize is > 0 and < 100 ? pageSize : 10;

      return new QueryParams(pageValue, pageSize, searchValue, orderBy, orderDirection);
    }


    public record QueryParams(int PageValue, int pageSize, string? SearchValue, string? OrderBy, string? OrderDirection);
  }
}
