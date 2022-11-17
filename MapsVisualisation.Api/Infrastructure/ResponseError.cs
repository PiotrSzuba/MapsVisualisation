namespace MapsVisualisation.Api.Infrastructure;

public class ResponseError
{
    public string Message { get; set; }

    public ResponseError(string message)
    {
        Message = message;
    }
}
