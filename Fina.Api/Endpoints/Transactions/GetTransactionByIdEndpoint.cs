using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Categories;

public class GetTransactionByIdEndpoint : IEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/{id}", HandleAsync)
        .WithName("Transactions: Get By ID")
        .WithSummary("Finds transaction by ID")
        .WithDescription("Finds transaction by ID")
        .WithOrder(3)
        .Produces<Response<Transaction?>>();
  // .RequireAuthorization(); // Blocks the request if the user is not authenticated

  private static async Task<IResult> HandleAsync(
    // Ps: how to get a logged user
    // ClaimsPrincipal user,
    ITransactionHandler handler,
    long id)
  {
    var request = new GetTransactionByIdRequest
    {
      // UserId = user.Identity?.Name ?? string.Empty,
      UserId = ApiConfiguration.UserId,
      Id = id
    };

    var result = await handler.GetByIdAsync(request);
    return result.IsSuccess
      ? TypedResults.Ok(result)
      : TypedResults.BadRequest(result);
  }
}
