using Fina.Api.Common.Api;
using Fina.Api.Endpoints.Categories;
using Fina.Api.Endpoints.Transactions;

namespace Fina.Api.Endpoints;

public static class Endpoint
{
  public static void MapEndpoints(this WebApplication app)
  {
    var endpoints = app.MapGroup("");

    endpoints.MapGroup("/")
      .WithTags("Health Check")
      .MapGet("/", () => new { message = "OK!" });

    endpoints.MapGroup("v1/categories")
      .WithTags("Categories")
      .MapEndpoint<CreateCategoryEndpoint>()
      .MapEndpoint<DeleteCategoryEndpoint>()
      .MapEndpoint<GetAllCategoriesEndpoint>()
      .MapEndpoint<GetCategoryByIdEndpoint>()
      .MapEndpoint<UpdateCategoryEndpoint>();

    endpoints.MapGroup("v1/transactions")
      .WithTags("Transactions")
      .MapEndpoint<CreateTransactionEndpoint>()
      .MapEndpoint<DeleteTransactionEndpoint>()
      .MapEndpoint<GetTransactionByIdEndpoint>()
      .MapEndpoint<GetTransactionsByPeriodEndpoint>()
      .MapEndpoint<UpdateTransactionEndpoint>();
  }

  private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
    // not mandatory, but interesting here
    where TEndpoint : IEndpoint
  {
    TEndpoint.Map(app);
    return app;
  }
}
