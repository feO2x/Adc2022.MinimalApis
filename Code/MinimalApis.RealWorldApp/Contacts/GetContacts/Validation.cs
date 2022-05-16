using System.Diagnostics.CodeAnalysis;
using Light.Validation;
using Light.Validation.Checks;
using Light.Validation.Tools;

namespace MinimalApis.RealWorldApp.Contacts.GetContacts;

public static class Validation
{
    public static bool CheckForPagingErrors(this ValidationContext context,
                                            int skip,
                                            int take,
                                            [NotNullWhen(true)] out object? errors)
    {
        context.Check(skip).IsGreaterThanOrEqualTo(0);
        context.Check(take).IsIn(Range.FromInclusive(1).ToInclusive(100));
        return context.TryGetErrors(out errors);
    }
}