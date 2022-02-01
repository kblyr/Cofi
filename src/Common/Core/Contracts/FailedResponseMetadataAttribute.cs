namespace Cofi.Contracts;

[AttributeUsage(AttributeTargets.Class)]
public class FailedResponseMetadataAttribute : Attribute
{
    public string ErrorType { get; }

    public FailedResponseMetadataAttribute(string errorType)
    {
        ErrorType = errorType;
    }
}