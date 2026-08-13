namespace StoreBlazor.Services;

public class SelectedProductState
{
  public string? ProductName { get; private set; }

  public event Action? OnChange;

  public void SetSelectedProduct(
      string productName)
  {
    ProductName =
        productName;

    NotifyStateChanged();
  }

  public void Clear()
  {
    ProductName =
        null;

    NotifyStateChanged();
  }

  private void NotifyStateChanged()
  {
    OnChange?.Invoke();
  }
}