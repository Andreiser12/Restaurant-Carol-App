using System;

namespace RestaurantCarol.Infrastructure.ExceptionHandling;

public abstract class BadRequestException : Exception
{
    protected BadRequestException(string message) : base(message)
    {
    }
}

public class ValidationBadRequestException : BadRequestException
{
    public ValidationBadRequestException(string message) : base(message)
    {
    }
}
