using System.ComponentModel.DataAnnotations;

namespace StoreBlazor.Models;

public class UpdateProductRequest
{
  [Required(ErrorMessage = "Product name is required.")]
  [StringLength(
      100,
      MinimumLength = 3,
      ErrorMessage = "Product name must be between 3 and 100 characters.")]
  public string Name { get; set; } = "";

  [Range(
      0.01,
      double.MaxValue,
      ErrorMessage = "Price must be greater than 0.")]
  public decimal Price { get; set; }

  [Range(
      1,
      int.MaxValue,
      ErrorMessage = "Please select a category.")]
  public int CategoryId { get; set; }
}