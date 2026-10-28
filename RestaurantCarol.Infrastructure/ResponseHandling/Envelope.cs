using System;

namespace RestaurantCarol.Infrastructure.ResponseHandling;

public class Envelope<T>
{
    public T? Result { get; }
    public string? ErrorMessage { get; }
    public DateTime TimeGenerated { get; }

    protected internal Envelope(T? result, string? errorMessage)
    {
        Result = result;
        ErrorMessage = errorMessage;
        TimeGenerated = DateTime.UtcNow;
    }
}

public class Envelope : Envelope<string>
{
    protected Envelope(string? errorMessage) : base(null, errorMessage)
    {
    }

    public static Envelope<T> Ok<T>(T result) => new(result, null);
    public static Envelope Ok() => new(null);
    public static Envelope Error(string errorMessage) => new(errorMessage);
    public static Envelope<T> Error<T>(string errorMessage) => new(default, errorMessage);
}
