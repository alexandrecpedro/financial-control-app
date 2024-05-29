using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Categories;

public class UpdateCategoryEndpoint : IEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
    => app.MapPut("/{id}", HandleAsync)
        .WithName("Categories: Update")
        .WithSummary("Updates a category")
        .WithDescription("Updates a category")
        .WithOrder(5)
        .Produces<Response<Category?>>();
  // .RequireAuthorization(); // Blocks the request if the user is not authenticated

  private static async Task<IResult> HandleAsync(
    // Ps: how to get a logged user
    // ClaimsPrincipal user,
    ICategoryHandler handler,
    UpdateCategoryRequest request,
    long id)
  {
    // request.UserId = user.Identity?.Name ?? string.Empty;
    request.UserId = ApiConfiguration.UserId;
    request.Id = id;

    var result = await handler.UpdateAsync(request);
    return result.IsSuccess
      ? TypedResults.Ok(result)
      : TypedResults.BadRequest(result);
  }
}
