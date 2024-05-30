using System.Net;
using System.Net.Http.Json;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;

namespace Fina.Web.Handlers;

public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(
        WebConfiguration.HttpClientName
    );
    private const string requestPath = "v1/categories";

    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync(requestPath, request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
            ?? new Response<Category?>(
                null,
                HttpStatusCode.BadRequest.GetHashCode(),
                "Unable to create category!"
            );
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        var result = await _httpClient.DeleteAsync($"{requestPath}/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
            ?? new Response<Category?>(
                null,
                HttpStatusCode.BadRequest.GetHashCode(),
                "Unable to delete the category!"
            );
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(
        GetAllCategoriesRequest request
    ) =>
        await _httpClient.GetFromJsonAsync<PagedResponse<List<Category>?>>($"{requestPath}")
        ?? new PagedResponse<List<Category>?>(
            null,
            HttpStatusCode.BadRequest.GetHashCode(),
            "Unable to retrieve the category!"
        );

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request) =>
        await _httpClient.GetFromJsonAsync<Response<Category?>>($"{requestPath}/{request.Id}")
        ?? new Response<Category?>(
            null,
            HttpStatusCode.BadRequest.GetHashCode(),
            "Unable to retrieve the category!"
        );

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        var result = await _httpClient.PutAsJsonAsync($"{requestPath}/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
            ?? new Response<Category?>(
                null,
                HttpStatusCode.BadRequest.GetHashCode(),
                "Unable to update category!"
            );
    }
}
