using Light.Validation;
using Light.Validation.Checks;

namespace MinimalApis.RealWorldApp.Contacts.NewContact;

public static class Validation
{
    public static Check<string> IsZipCode(this Check<string> check)
    {
        if (check.Value.Length != 5)
            check.AddError($"{check.Key} must be a ZIP code");
        return check.ContainsOnlyDigits(c => $"{c.Key} must be a ZIP code");
    }
}