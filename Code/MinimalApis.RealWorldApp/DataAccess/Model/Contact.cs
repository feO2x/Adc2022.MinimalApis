namespace MinimalApis.RealWorldApp.DataAccess.Model;

public sealed class Contact
{
    public int Id { get; set; }
    public string FirstName{ get; set; } = string.Empty;
    public string LastName{ get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Address? Address { get; set; }
}