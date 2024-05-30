using Fina.Core.Handlers;
using Fina.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Fina.Web.Pages.Categories;

public partial class CreateCategoryPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public CreateCategoryRequest InputModel { get; set; } = new();

    #endregion

    #region Services

    // It is not possible to inject a service by page constructor on .NET 8
    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;

    // Navigate using C# (redirect user between pages)
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    // Allows toasts, notifications, dialogs
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await Handler.CreateAsync(InputModel);
            if (result.IsSuccess)
            {
                // Green pop-up on screenS
                Snackbar.Add(result.Message, Severity.Success);
                NavigationManager.NavigateTo("/categories");
            }
            else
            {
                Snackbar.Add(result.Message, Severity.Error);
            }
        }
        catch (Exception ex)
        {
            // Red pop-up on screen
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
}
