namespace Cofi.Validation;

public interface IAccessValidationConfiguration<TResource> where TResource : notnull
{
    void Configure(IAccessValidationContext context, TResource resource);
}
