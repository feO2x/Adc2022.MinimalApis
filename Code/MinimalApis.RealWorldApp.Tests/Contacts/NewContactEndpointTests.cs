using System.Threading.Tasks;
using FluentAssertions;
using Light.Validation;
using MinimalApis.RealWorldApp.Contacts;
using MinimalApis.RealWorldApp.Contacts.NewContact;
using MinimalApis.RealWorldApp.DataAccess.Model;
using MinimalApis.RealWorldApp.Tests.TestHelpers;
using Serilog;
using Synnotech.AspNetCore.MinimalApis.Responses;
using Synnotech.DatabaseAbstractions.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace MinimalApis.RealWorldApp.Tests.Contacts;

public sealed class NewContactEndpointTests
{
    public NewContactEndpointTests(ITestOutputHelper output)
    {
        Output = output;
        Logger = output.CreateTestLogger();
        Session = new NewContactSessionMock();
        SessionFactory = new SessionFactoryMock<INewContactSession>(Session);
        Endpoint = new NewContactEndpoint(
            SessionFactory,
            new NewContactDtoValidator(ValidationContextFactory.Instance),
            Logger
        );
    }

    private ITestOutputHelper Output { get; }
    private ILogger Logger { get; }
    private NewContactSessionMock Session { get; }
    private SessionFactoryMock<INewContactSession> SessionFactory { get; }
    private NewContactEndpoint Endpoint { get; }

    [Fact]
    public async Task CreateNewContact()
    {
        var newContact = CreateNewContactDto();

        var result = await Endpoint.CreateContact(newContact);

        var expectedResponseBody = CreateExpectedContactDetailDto(newContact);
        result.GetBody<ContactDetailDto>().Should().Be(expectedResponseBody);
        result.ShouldBe201Created();
        result.GetLocationUrl().Should().Be("/api/contacts/1");
        var expectedContact = CreateExpectedContact(newContact);
        Session.CapturedContact.Should().BeEquivalentTo(expectedContact);
        Session.CapturedAddress.Should().BeEquivalentTo(expectedContact.Address);
        Session.ContactIdCount.Should().Be(2);
        Session.AddressIdCount.Should().Be(2);
        Session.SaveChangesMustHaveBeenCalled()
               .MustBeDisposed();

        static NewContactDto CreateNewContactDto() =>
            new ()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@work.com",
                Street = "592 Waterfall Street",
                ZipCode = "58112",
                Location = "Home Town"
            };

        static Contact CreateExpectedContact(NewContactDto newContact) =>
            new ()
            {
                Id = 1,
                FirstName = newContact.FirstName,
                LastName = newContact.LastName,
                Email = newContact.Email,
                Address = new Address
                {
                    Id = 1,
                    Street = newContact.Street,
                    ZipCode = newContact.ZipCode,
                    Location = newContact.Location,
                    ContactId = 1
                }
            
            };

        static ContactDetailDto CreateExpectedContactDetailDto(NewContactDto newContact) =>
            new (1, newContact.FirstName, newContact.LastName, newContact.Email, newContact.Street, newContact.ZipCode, newContact.Location);
    }

    [Theory]
    [MemberData(nameof(InvalidDtos))]
    public async Task InvalidDto(NewContactDto invalidDto)
    {
        var result = await Endpoint.CreateContact(invalidDto);

        Output.WriteBodyAsJson(result);
        result.ShouldBe400BadRequest();
        SessionFactory.OpenSessionMustNotHaveBeenCalled();
    }

    public static readonly TheoryData<NewContactDto> InvalidDtos =
        new ()
        {
            null!,
            new () { FirstName = "" },
            new () { FirstName = "John", LastName = "" },
            new () { FirstName = "John", LastName = "Doe", Email = "This is no email" },
            new () { FirstName = "John", LastName = "Doe", Email = "john.doe@gmail.com", Street = "f"}
        };

    private sealed class NewContactSessionMock : AsyncSessionMock, INewContactSession
    {
        public int ContactIdCount { get; private set; } = 1;
        public int AddressIdCount { get; private set; } = 1;
        public Contact? CapturedContact { get; private set; }
        public Address? CapturedAddress { get; private set; }

        public Task<int> InsertContactAsync(Contact contact)
        {
            CapturedContact = contact;
            return Task.FromResult(ContactIdCount++);
        }

        public Task<int> InsertAddressAsync(Address address)
        {
            CapturedAddress = address;
            return Task.FromResult(AddressIdCount++);
        }
    }
}