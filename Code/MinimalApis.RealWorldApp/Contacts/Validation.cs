using System.Diagnostics.CodeAnalysis;
using Light.Validation;
using Light.Validation.Checks;
using Light.Validation.Tools;

namespace MinimalApis.RealWorldApp.Contacts;

public static class Validation
{
    public static bool CheckForIdErrors(this ValidationContext context,
                                        int id,
                                        [NotNullWhen(true)] out object? errors)
    {
        context.Check(id).IsGreaterThan(0);
        return context.TryGetErrors(out errors);
    }

    public static Check<string> IsZipCode(this Check<string> check)
    {
        if (check.Value.Length != 5)
            check.AddError($"{check.Key} must be a ZIP code");
        return check.ContainsOnlyDigits(c => $"{c.Key} must be a ZIP code");
    }

    public static void ValidateContactProperties(this ValidationContext context, NewContactDto dto)
    {
        var range = Range.FromInclusive(2).ToInclusive(100);
        dto.FirstName = context.Check(dto.FirstName).HasLengthIn(range);
        dto.LastName = context.Check(dto.LastName).HasLengthIn(range);
        dto.Email = context.Check(dto.Email).IsEmail().IsShorterThan(100);
        dto.Street = context.Check(dto.Street).HasLengthIn(range);
        dto.ZipCode = context.Check(dto.ZipCode).IsZipCode();
        dto.Location = context.Check(dto.Location).HasLengthIn(range);
    }

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