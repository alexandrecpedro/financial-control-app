using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Categories;

public class DeleteCategoryEndpoint : IEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
    => app.MapDelete("/{id}", HandleAsync)
        .WithName("Categories: Delete")
        .WithSummary("Deletes a category")
        .WithDescription("Deletes a category")
        .WithOrder(2)
        .Produces<Response<Category?>>();
  // .RequireAuthorization(); // Blocks the request if the user is not authenticated

  private static async Task<IResult> HandleAsync(
    // Ps: how to get a logged user
    // ClaimsPrincipal user,
    ICategoryHandler handler,
    long id)
  {
    var request = new DeleteCategoryRequest
    {
      // UserId = user.Identity?.Name ?? string.Empty,
      UserId = ApiConfiguration.UserId,
      Id = id
    };

    var result = await handler.DeleteAsync(request);
    return result.IsSuccess
      ? TypedResults.Ok(result)
      : TypedResults.BadRequest(result);
  }
}
