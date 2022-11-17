namespace MapsVisualisation.Domain.Helpers;

public static class Errors
{
    public static class General
    {
        public static string EmptyTable<T>() where T : class => $"Data about {typeof(T).Name}s does not exists.";
        public static string NotFound<T>(object id) where T : class => $"{typeof(T).Name} with id '{id}' does not exists.";
    }
}
