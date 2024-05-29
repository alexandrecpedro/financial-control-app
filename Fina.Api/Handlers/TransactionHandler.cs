using System.Net;
using Fina.Api.Data;
using Fina.Core.Common;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Transactions;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
  public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
  {
    if (request is { Type: Fina.Core.Enums.ETransactionType.Withdraw, Amount: >= 0 })
      request.Amount *= -1;

    try
    {
      var transaction = new Transaction
      {
        UserId = request.UserId,
        CategoryId = request.CategoryId,
        CreatedAt = DateTime.Now,
        Amount = request.Amount,
        PaidOrReceivedAt = request.PaidOrReceivedAt,
        Title = request.Title,
        Type = request.Type
      };

      await context.Transactions.AddAsync(transaction);
      await context.SaveChangesAsync();

      return new Response<Transaction?>(
        transaction,
        HttpStatusCode.Created.GetHashCode(),
        "Transaction created successfully!");
    }
    catch
    {
      // TODO: Use Serilog, OpenTelemetry
      return new Response<Transaction?>(
        null,
        HttpStatusCode.InternalServerError.GetHashCode(),
        "It was not possible to create the transaction!");
    }
  }

  public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
  {
    try
    {
      var transaction = await context
        .Transactions
        .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

      if (transaction is null)
        return new Response<Transaction?>(
          null,
          HttpStatusCode.NotFound.GetHashCode(),
          "Transaction not found!");

      context.Transactions.Remove(transaction);
      await context.SaveChangesAsync();

      return new Response<Transaction?>(
        transaction,
        HttpStatusCode.NoContent.GetHashCode(),
        "Transaction deleted successfully!");
    }
    catch
    {
      // TODO: Use Serilog, OpenTelemetry
      return new Response<Transaction?>(
        null,
        HttpStatusCode.InternalServerError.GetHashCode(),
        "It was not possible to delete the transaction!");
    }
  }

  public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
  {
    try
    {
      var transaction = await context
        .Transactions
        .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

      return transaction is null
      ? new Response<Transaction?>(null, HttpStatusCode.NotFound.GetHashCode(), "Transaction not found!")
      : new Response<Transaction?>(transaction);
    }
    catch
    {
      // TODO: Use Serilog, OpenTelemetry
      return new Response<Transaction?>(
        null,
        HttpStatusCode.InternalServerError.GetHashCode(),
        "It was not possible to find the transaction!");
    }
  }

  public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
  {
    try
    {
      request.StartDate ??= DateTime.Now.GetFirstDay();
      request.EndDate ??= DateTime.Now.GetLastDay();
    }
    catch
    {
      // TODO: Use Serilog, OpenTelemetry
      return new PagedResponse<List<Transaction>?>(
      null,
      HttpStatusCode.InternalServerError.GetHashCode(),
      "It was not possible to determine the start or end date!");
    }

    try
    {
      var query = context
          .Transactions
          .AsNoTracking()
          .Where(x =>
            x.PaidOrReceivedAt >= request.StartDate &&
            x.PaidOrReceivedAt <= request.EndDate &&
            x.UserId == request.UserId)
          .OrderBy(x => x.PaidOrReceivedAt);

      var transactions = await query
          .Skip((request.PageNumber - 1) * request.PageSize)
          .Take(request.PageSize)
          .ToListAsync();

      var count = await query.CountAsync();

      return new PagedResponse<List<Transaction>?>(
        transactions,
        count,
        request.PageNumber,
        request.PageSize);
    }
    catch
    {
      return new PagedResponse<List<Transaction>?>(
        null,
        HttpStatusCode.InternalServerError.GetHashCode(),
        "It was not possible to query the transactions!");
    }
  }

  public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
  {
    if (request is { Type: Fina.Core.Enums.ETransactionType.Withdraw, Amount: >= 0 })
      request.Amount *= -1;

    try
    {
      var transaction = await context
        .Transactions
        .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

      if (transaction is null)
        return new Response<Transaction?>(
          null,
          HttpStatusCode.NotFound.GetHashCode(),
          "Transaction not found!");

      transaction.CategoryId = request.CategoryId;
      transaction.Amount = request.Amount;
      transaction.Title = request.Title;
      transaction.Type = request.Type;
      transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;

      context.Transactions.Update(transaction);
      await context.SaveChangesAsync();

      return new Response<Transaction?>(transaction);
    }
    catch
    {
      return new Response<Transaction?>(
        null,
        HttpStatusCode.InternalServerError.GetHashCode(),
        "It was not possible to update the transaction!"
      );
    }
  }
}
