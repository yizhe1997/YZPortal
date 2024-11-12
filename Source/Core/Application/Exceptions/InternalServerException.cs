namespace Application.Exceptions;

public class InternalServerException(string message) : Exception(message)
{
}