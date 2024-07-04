namespace Application.Exceptions
{
    public class SyncException(string message, Exception innerException) : Exception(message, innerException)
    {
    }
}
