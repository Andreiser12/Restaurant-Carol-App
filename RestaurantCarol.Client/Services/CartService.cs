using RestaurantCarol.Shared.DTOs.Menu;

namespace RestaurantCarol.Client.Services;

public class CartService
{
    private readonly List<CartItemDto> _items = new();

    public IReadOnlyList<CartItemDto> Items => _items.AsReadOnly();

    public int TotalCount => _items.Sum(i => i.Quantity);

    public decimal TotalPrice => _items.Sum(i => i.Subtotal);

    public event Action? OnChange;

    public void AddToCart(DishDto dish, int quantity = 1)
    {
        if (quantity <= 0) return;

        var existing = _items.FirstOrDefault(i => i.DishId == dish.Id);
        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            _items.Add(new CartItemDto
            {
                DishId = dish.Id,
                Name = dish.Name,
                Price = dish.Price,
                Quantity = quantity,
                ImagePath = dish.ImagePath
            });
        }

        NotifyStateChanged();
    }

    public void UpdateQuantity(int dishId, int newQuantity)
    {
        var existing = _items.FirstOrDefault(i => i.DishId == dishId);
        if (existing != null)
        {
            if (newQuantity <= 0)
            {
                _items.Remove(existing);
            }
            else
            {
                existing.Quantity = newQuantity;
            }
            NotifyStateChanged();
        }
    }

    public void RemoveFromCart(int dishId)
    {
        var existing = _items.FirstOrDefault(i => i.DishId == dishId);
        if (existing != null)
        {
            _items.Remove(existing);
            NotifyStateChanged();
        }
    }

    public void Clear()
    {
        _items.Clear();
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
