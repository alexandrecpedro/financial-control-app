namespace Fina.Api.Common.Api;

public static class AppExtension
{
  // Enabling SWAGGER only on DEV mode
  public static void ConfigureDevEnvironment(this WebApplication app)
  {
    app.UseSwagger();
    app.UseSwaggerUI();
    // Using authorization on SWAGGER
    // app.MapSwagger().RequireAuthorization();
  }
}
