using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
    => app.MapPost("/", HandleAsync)
        .WithName("Categories: Create")
        .WithSummary("Creates a new category")
        .WithDescription("Creates a new category")
        .WithOrder(1)
        .Produces<Response<Category?>>();
  // .RequireAuthorization(); // Blocks the request if the user is not authenticated

  // Execute the handler
  private static async Task<IResult> HandleAsync(
    // Ps: how to get a logged user
    // ClaimsPrincipal user,
    ICategoryHandler handler,
    CreateCategoryRequest request)
  {
    // request.UserId = user.Identity?.Name ?? string.Empty;
    request.UserId = ApiConfiguration.UserId;
    var result = await handler.CreateAsync(request);
    return result.IsSuccess
      ? TypedResults.Created($"/{result.Data?.Id}", result)
      : TypedResults.BadRequest(result);
  }
}
