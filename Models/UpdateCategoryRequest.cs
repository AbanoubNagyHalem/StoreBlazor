using System.ComponentModel.DataAnnotations;

namespace StoreBlazor.Models;

public class UpdateCategoryRequest
{
  [Required(
      ErrorMessage = "Category name is required.")]
  [StringLength(
      100,
      MinimumLength = 3,
      ErrorMessage =
          "Category name must be between 3 and 100 characters.")]
  public string Name { get; set; } = "";
}