using System.ComponentModel.DataAnnotations;

namespace StoreBlazor.Models;

public class CreateProductRequest
{
  [Required]
  [StringLength(
      100,
      MinimumLength = 3)]
  public string Name { get; set; } = "";

  [Range(
      0.01,
      double.MaxValue,
      ErrorMessage = "Price must be greater than zero.")]
  public decimal Price { get; set; }

  [Range(
      1,
      int.MaxValue,
      ErrorMessage = "Please select a category.")]
  public int CategoryId { get; set; }
}