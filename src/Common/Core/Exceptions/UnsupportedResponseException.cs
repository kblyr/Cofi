using System.Runtime.Serialization;

namespace Cofi;

public class UnsupportedResponseException : CofiException
{
    public UnsupportedResponseException()
    {
    }

    public UnsupportedResponseException(string? message) : base(message)
    {
    }

    public UnsupportedResponseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UnsupportedResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}