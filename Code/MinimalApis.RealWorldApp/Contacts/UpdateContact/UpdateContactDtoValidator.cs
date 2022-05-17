using Light.Validation;
using Light.Validation.Checks;

namespace MinimalApis.RealWorldApp.Contacts.UpdateContact;

public sealed class UpdateContactDtoValidator : Validator<UpdateContactDto>
{
    public UpdateContactDtoValidator(IValidationContextFactory validationContextFactory)
        : base(validationContextFactory) { }

    protected override UpdateContactDto PerformValidation(ValidationContext context, UpdateContactDto dto)
    {
        context.Check(dto.Id).IsGreaterThan(0);
        context.ValidateContactProperties(dto);
        return dto;
    }
}