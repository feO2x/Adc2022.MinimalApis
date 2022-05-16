using Light.Validation;
using Light.Validation.Checks;
using Light.Validation.Tools;

namespace MinimalApis.RealWorldApp.Contacts.NewContact;

public sealed class NewContactDtoValidator : Validator<NewContactDto>
{
    public NewContactDtoValidator(IValidationContextFactory validationContextFactory)
        : base(validationContextFactory) { }

    protected override NewContactDto PerformValidation(ValidationContext context, NewContactDto dto)
    {
        var range = Range.FromInclusive(2).ToInclusive(100);
        dto.FirstName = context.Check(dto.FirstName).HasLengthIn(range);
        dto.LastName = context.Check(dto.LastName).HasLengthIn(range);
        dto.Email = context.Check(dto.Email).IsEmail().IsShorterThan(100);
        dto.Street = context.Check(dto.Street).HasLengthIn(range);
        dto.ZipCode = context.Check(dto.ZipCode).IsZipCode();
        dto.Location = context.Check(dto.Location).HasLengthIn(range);
        return dto;
    }
}