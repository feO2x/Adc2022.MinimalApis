namespace MinimalApis.RealWorldApp.DataAccess.Model;

public sealed class Address
{
    public int Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int ContactId { get; set; }
    public Contact? Contact { get; set; }
}