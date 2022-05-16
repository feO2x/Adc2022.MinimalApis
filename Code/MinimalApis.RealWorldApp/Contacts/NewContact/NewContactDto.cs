namespace MinimalApis.RealWorldApp.Contacts.NewContact;

public sealed class NewContactDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
}