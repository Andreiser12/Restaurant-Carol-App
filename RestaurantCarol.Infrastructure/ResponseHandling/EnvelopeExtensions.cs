using Microsoft.AspNetCore.Mvc;

namespace RestaurantCarol.Infrastructure.ResponseHandling;

public static class EnvelopeExtensions
{
    public static Envelope<T> ToEnvelope<T>(this T result)
    {
        return Envelope.Ok(result);
    }

    public static OkObjectResult OkEnvelope<T>(this ControllerBase controller, T result)
    {
        return controller.Ok(Envelope.Ok(result));
    }

    public static OkObjectResult OkEnvelope(this ControllerBase controller)
    {
        return controller.Ok(Envelope.Ok());
    }

    public static ObjectResult ErrorEnvelope(this ControllerBase controller, string errorMessage, int statusCode = 400)
    {
        return controller.StatusCode(statusCode, Envelope.Error(errorMessage));
    }

    public static ObjectResult ErrorEnvelope<T>(this ControllerBase controller, string errorMessage, int statusCode = 400)
    {
        return controller.StatusCode(statusCode, Envelope.Error<T>(errorMessage));
    }
}
