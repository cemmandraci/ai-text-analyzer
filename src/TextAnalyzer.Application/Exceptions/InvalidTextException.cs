namespace TextAnalyzer.Application.Exceptions;

public class InvalidTextException : Exception
{
    public InvalidTextException(string message) : base(message) { }
}