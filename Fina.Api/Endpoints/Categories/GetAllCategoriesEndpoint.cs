using Fina.Api.Common.Api;
using Fina.Core;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Categories;

public class GetAllCategoriesEndpoint : IEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/", HandleAsync)
        .WithName("Categories: Get All")
        .WithSummary("Finds all categories")
        .WithDescription("Finds all categories")
        .WithOrder(3)
        .Produces<PagedResponse<List<Category>?>>();
  // .RequireAuthorization(); // Blocks the request if the user is not authenticated

  private static async Task<IResult> HandleAsync(
    // Ps: how to get a logged user
    // ClaimsPrincipal user,
    ICategoryHandler handler,
    // http://localhost:8080/v1/categories?pageNumber=1&pageSize=25
    [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
    [FromQuery] int pageSize = Configuration.DefaultPageSize)
  {
    var request = new GetAllCategoriesRequest
    {
      // UserId = user.Identity?.Name ?? string.Empty,
      UserId = ApiConfiguration.UserId,
      PageNumber = pageNumber,
      PageSize = pageSize
    };

    var result = await handler.GetAllAsync(request);
    return result.IsSuccess
      ? TypedResults.Ok(result)
      : TypedResults.BadRequest(result);
  }
}
