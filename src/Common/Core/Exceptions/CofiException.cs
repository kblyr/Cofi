using System;
using System.Runtime.Serialization;

namespace Cofi.Exceptions;

public class CofiException : Exception
{
    public CofiException()
    {
    }

    public CofiException(string message) : base(message)
    {
    }

    public CofiException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected CofiException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}