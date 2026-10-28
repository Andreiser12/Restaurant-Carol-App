using System;

namespace RestaurantCarol.Infrastructure.ExceptionHandling;

public abstract class NotFoundException : Exception
{
    protected NotFoundException(string message) : base(message)
    {
    }
}

public class ItemNotFoundException : NotFoundException
{
    public ItemNotFoundException(string itemName, object key)
        : base($"{itemName} with key '{key}' was not found.")
    {
    }
}
