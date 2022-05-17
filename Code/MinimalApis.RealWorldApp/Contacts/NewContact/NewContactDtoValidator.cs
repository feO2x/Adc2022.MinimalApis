using Light.Validation;

namespace MinimalApis.RealWorldApp.Contacts.NewContact;

public sealed class NewContactDtoValidator : Validator<NewContactDto>
{
    public NewContactDtoValidator(IValidationContextFactory validationContextFactory)
        : base(validationContextFactory) { }

    protected override NewContactDto PerformValidation(ValidationContext context, NewContactDto dto)
    {
        context.ValidateContactProperties(dto);
        return dto;
    }
}