namespace Cofi.Contracts;

public class FailedResponseMetadataAttribute : Attribute
{
    public string ErrorName { get; }

    public FailedResponseMetadataAttribute(string errorName)
    {
        ErrorName = errorName;
    }
}