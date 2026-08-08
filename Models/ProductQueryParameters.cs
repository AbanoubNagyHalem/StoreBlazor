namespace StoreBlazor.Models;

public class ProductQueryParameters
{
  public string? Search { get; set; }

  public int? CategoryId { get; set; }

  public string? SortBy { get; set; }

  public string SortDirection { get; set; } = "asc";

  public int Page { get; set; } = 1;

  public int PageSize { get; set; } = 3;
}