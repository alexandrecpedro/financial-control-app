using System.ComponentModel.DataAnnotations;

namespace Fina.Core.Requests.Categories;

public class CreateCategoryRequest : Request
{
    [Required(ErrorMessage = "Invalid title")]
    [MaxLength(80, ErrorMessage = "Title must contain no more than 80 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Invalid description")]
    public string Description { get; set; } = string.Empty;
}
