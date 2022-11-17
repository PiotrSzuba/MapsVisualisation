namespace MapsVisualisation.Domain.Exceptions;

public class EntityInvalidStateException : Exception
{
    public EntityInvalidStateException() : base() { }

    public EntityInvalidStateException(string message) : base(message) { }
}

