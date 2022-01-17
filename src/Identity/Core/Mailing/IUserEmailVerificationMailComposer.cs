using Cofi.Mailing;

namespace Cofi.Identity.Mailing;

public interface IUserEmailVerificationMailComposer
{
    ComposedMail Compose(SendUserEmailVerification payload);
}