using Fina.Api.Common.Api;
using Fina.Core;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fina.Api.Endpoints.Transactions;

public class GetTransactionsByPeriodEndpoint : IEndpoint
{
  public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/", HandleAsync)
        .WithName("Transactions: Get All")
        .WithSummary("Finds all transactions by period")
        .WithDescription("Finds all transactions by period")
        .WithOrder(4)
        .Produces<PagedResponse<List<Transaction>?>>();
  // .RequireAuthorization(); // Blocks the request if the user is not authenticated

  private static async Task<IResult> HandleAsync(
    // Ps: how to get a logged user
    // ClaimsPrincipal user,
    ITransactionHandler handler,
    [FromQuery] DateTime? startDate = null,
    [FromQuery] DateTime? endDate = null,
    [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
    [FromQuery] int pageSize = Configuration.DefaultPageSize)
  {
    var request = new GetTransactionsByPeriodRequest
    {
      // UserId = user.Identity?.Name ?? string.Empty,
      UserId = ApiConfiguration.UserId,
      PageNumber = pageNumber,
      PageSize = pageSize,
      StartDate = startDate,
      EndDate = endDate
    };

    var result = await handler.GetByPeriodAsync(request);
    return result.IsSuccess
      ? TypedResults.Ok(result)
      : TypedResults.BadRequest(result);
  }
}
