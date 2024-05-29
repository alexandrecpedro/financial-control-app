using Fina.Api.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Transactions;

public class UpdateTransactionEndpoint : IEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
    => app.MapPut("/{id}", HandleAsync)
        .WithName("Transactions: Update")
        .WithSummary("Updates a transaction")
        .WithDescription("Updates a transaction")
        .WithOrder(5)
        .Produces<Response<Transaction?>>();
  // .RequireAuthorization(); // Blocks the request if the user is not authenticated

  private static async Task<IResult> HandleAsync(
    // Ps: how to get a logged user
    // ClaimsPrincipal user,
    ITransactionHandler handler,
    UpdateTransactionRequest request,
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
