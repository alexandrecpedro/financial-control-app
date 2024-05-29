using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Transactions;

public class DeleteTransactionEndpoint : IEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
    => app.MapDelete("/{id}", HandleAsync)
        .WithName("Transactions: Delete")
        .WithSummary("Deletes a transaction")
        .WithDescription("Deletes a transaction")
        .WithOrder(2)
        .Produces<Response<Transaction?>>();
  // .RequireAuthorization(); // Blocks the request if the user is not authenticated

  private static async Task<IResult> HandleAsync(
    // Ps: how to get a logged user
    // ClaimsPrincipal user,
    ITransactionHandler handler,
    long id)
  {
    var request = new DeleteTransactionRequest
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
