using System.Net;
using Fina.Api.Data;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
  public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
  {
    var category = new Category
    {
      UserId = request.UserId,
      Title = request.Title,
      Description = request.Description
    };

    try
    {
      await context.Categories.AddAsync(category);
      await context.SaveChangesAsync();

      return new Response<Category?>(
        category,
        HttpStatusCode.Created.GetHashCode(),
        "Category created successfully!");
    }
    catch
    {
      // TODO: Use Serilog, OpenTelemetry
      return new Response<Category?>(
        null,
        HttpStatusCode.InternalServerError.GetHashCode(),
        "It was not possible to create the category!");
    }
  }

  public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
  {
    try
    {
      var category = await context
        .Categories
        .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

      if (category is null)
        return new Response<Category?>(
          null,
          HttpStatusCode.NotFound.GetHashCode(),
          "Category not found!");

      context.Categories.Remove(category);
      await context.SaveChangesAsync();

      return new Response<Category?>(
        category,
        HttpStatusCode.NoContent.GetHashCode(),
        "Category deleted successfully!");
    }
    catch
    {
      // TODO: Use Serilog, OpenTelemetry
      return new Response<Category?>(
        null,
        HttpStatusCode.InternalServerError.GetHashCode(),
        "It was not possible to delete the category!");
    }
  }

  public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
  {
    try
    {
      var query = context
          .Categories
          .AsNoTracking()
          .Where(x => x.UserId == request.UserId)
          .OrderBy(x => x.Title);

      // This query will only be materialized (returned in a list) in this method
      var categories = await query
          .Skip((request.PageNumber - 1) * request.PageSize)
          .Take(request.PageSize)
          .ToListAsync();

      var count = await query.CountAsync();

      return new PagedResponse<List<Category>?>(
        categories,
        count,
        request.PageNumber,
        request.PageSize);
    }
    catch
    {
      // TODO: Use Serilog, OpenTelemetry;
      return new PagedResponse<List<Category>?>(null, HttpStatusCode.InternalServerError.GetHashCode(), "It was not possible to query the categories!");
    }
  }

  public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
  {
    try
    {
      var category = await context
        .Categories
        .AsNoTracking() // uses less memory to process (ignore if any data has been changed)
        .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

      return category is null
      ? new Response<Category?>(null, HttpStatusCode.NotFound.GetHashCode(), "Category not found!")
      : new Response<Category?>(category);
    }
    catch
    {
      // TODO: Use Serilog, OpenTelemetry
      return new Response<Category?>(
        null,
        HttpStatusCode.InternalServerError.GetHashCode(),
        "It was not possible to find the category!");
    }
  }

  public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
  {
    try
    {
      var category = await context
        .Categories
        .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

      if (category is null)
        return new Response<Category?>(null, HttpStatusCode.NotFound.GetHashCode(), "Category not found!");

      category.Title = request.Title;
      category.Description = request.Description;

      context.Categories.Update(category);
      await context.SaveChangesAsync();

      return new Response<Category?>(category, message: "Category updated successfully!");
    }
    catch
    {
      // TODO: Use Serilog, OpenTelemetry
      return new Response<Category?>(
        null,
        HttpStatusCode.InternalServerError.GetHashCode(),
        "It was not possible to update the category!");
    }
  }
}
