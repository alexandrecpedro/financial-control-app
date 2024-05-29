using Fina.Api.Data;
using Fina.Api.Handlers;
using Fina.Core;
using Fina.Core.Handlers;
using Microsoft.EntityFrameworkCore;

namespace Fina.Api.Common.Api;

public static class BuildExtension
{
  // App Settings
  public static void AddConfiguration(this WebApplicationBuilder builder)
  {
    // TODO: Use secrets for connection string
    ApiConfiguration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
    Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
    Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;
  }

  // SWAGGER Docs
  public static void AddDocumentation(this WebApplicationBuilder builder)
  {
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(x =>
    {
      // Category -> Fina.Core.Models.Category
      // Transaction -> Fina.Core.Models.Transaction
      x.CustomSchemaIds(n => n.FullName);
    });
  }

  // Database settings
  public static void AddDataContexts(this WebApplicationBuilder builder)
  {
    builder
      .Services
      .AddDbContext<AppDbContext>(
        x =>
        {
          x.UseSqlServer(ApiConfiguration.ConnectionString);
        });
  }

  // CORS Access Policies settings
  // CORS -> Cross Origin Resource Sharing
  public static void AddCrossOrigin(this WebApplicationBuilder builder)
  {
    builder.Services.AddCors(
      options => options.AddPolicy(
        ApiConfiguration.CorsPolicyName,
        policy => policy
          .WithOrigins([
            Configuration.BackendUrl,
            Configuration.FrontendUrl,
          ])
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials()
      )
    );
  }

  // Services registration settings
  public static void AddServices(this WebApplicationBuilder builder)
  {
    builder
      .Services
      .AddTransient<ICategoryHandler, CategoryHandler>();

    builder
      .Services
      .AddTransient<ITransactionHandler, TransactionHandler>();
  }
}
