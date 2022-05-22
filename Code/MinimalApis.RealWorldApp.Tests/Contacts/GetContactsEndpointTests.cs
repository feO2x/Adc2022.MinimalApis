using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Light.Validation;
using Microsoft.AspNetCore.Http;
using MinimalApis.RealWorldApp.Contacts.GetContacts;
using MinimalApis.RealWorldApp.DataAccess.Model;
using MinimalApis.RealWorldApp.Tests.TestHelpers;
using Synnotech.AspNetCore.MinimalApis.Responses;
using Synnotech.DatabaseAbstractions.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace MinimalApis.RealWorldApp.Tests.Contacts;

public sealed class GetContactsEndpointTests
{
    public GetContactsEndpointTests(ITestOutputHelper output)
    {
        Output = output;
        Session = new GetContactsSessionMock();
        SessionFactory = new SessionFactoryMock<IGetContactsSession>(Session);
    }

    private ITestOutputHelper Output { get; }

    private IValidationContextFactory ValidationContextFactory { get; } =
        Light.Validation.ValidationContextFactory.Instance;

    private GetContactsSessionMock Session { get; }
    private SessionFactoryMock<IGetContactsSession> SessionFactory { get; }

    [Fact]
    public async Task GetContacts()
    {
        var result = await CallEndpointAsync();

        result.GetStatusCode().Should().Be(StatusCodes.Status200OK);
        var expectedContacts = ContactListDto.FromContacts(Session.Contacts);
        result.GetBody<ContactListDto[]>().Should().Equal(expectedContacts);
        Session.MustBeDisposed();
    }

    [Theory]
    [InlineData(-1, 30)]
    [InlineData(0, 101)]
    [InlineData(2500, 0)]
    [InlineData(-17543, 150)]
    public async Task InvalidPagingParameters(int invalidSkip, int invalidTake)
    {
        var result = await CallEndpointAsync(invalidSkip, invalidTake);

        Output.WriteBodyAsJson(result);
        result.ShouldBe400BadRequest();
        SessionFactory.OpenSessionMustNotHaveBeenCalled();
    }

    [Theory]
    [InlineData("  foo", "foo")]
    [InlineData("bar\t", "bar")]
    [InlineData("it's alright", "it's alright")]
    public async Task SearchTermMustBeNormalized(string searchTerm, string expectedSearchTerm)
    {
        await CallEndpointAsync(searchTerm: searchTerm);

        Session.CapturedSearchTerm.Should().Be(expectedSearchTerm);
    }

    private Task<IResult> CallEndpointAsync(int skip = 0, int take = 30, string? searchTerm = null) =>
        GetContactsEndpoint.GetContacts(ValidationContextFactory, SessionFactory, skip, take, searchTerm);

    private sealed class GetContactsSessionMock : AsyncReadOnlySessionMock, IGetContactsSession
    {
        public List<Contact> Contacts { get; } = new ()
        {
            new () { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@mail.com", Address = new () { Id = 1, Street = "123 Example Street", ZipCode = "51928", Location = "Sample Village" } },
            new () { Id = 2, FirstName = "Jane", LastName = "Shoe", Email = "jane.shoe@email.net", Address = new () { Id = 2, Street = "678 Flow Street", ZipCode = "39182", Location = "Ville" } }
        };

        public string? CapturedSearchTerm { get; private set; }

        public Task<List<Contact>> GetContactsAsync(int skip, int take, string? searchTerm)
        {
            CapturedSearchTerm = searchTerm;
            return Task.FromResult(Contacts);
        }
    }
}