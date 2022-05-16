using System.Diagnostics.CodeAnalysis;
using Light.Validation;
using Light.Validation.Checks;

namespace MinimalApis.RealWorldApp.Contacts.GetContactDetails;

public static class Validation
{
    public static bool CheckForIdErrors(this ValidationContext context,
                                        int id,
                                        [NotNullWhen(true)] out object? errors)
    {
        context.Check(id).IsGreaterThan(0);
        return context.TryGetErrors(out errors);
    }
}