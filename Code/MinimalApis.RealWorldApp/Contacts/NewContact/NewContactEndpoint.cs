using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MinimalApis.RealWorldApp.DataAccess.Model;
using MinimalApis.RealWorldApp.Infrastructure;
using Serilog;
using Synnotech.AspNetCore.MinimalApis.Responses;
using Synnotech.DatabaseAbstractions;

namespace MinimalApis.RealWorldApp.Contacts.NewContact;

public sealed class NewContactEndpoint : IMinimalApiEndpoint
{
    public NewContactEndpoint(ISessionFactory<INewContactSession> sessionFactory,
                              NewContactDtoValidator validator,
                              ILogger logger)
    {
        SessionFactory = sessionFactory;
        Validator = validator;
        Logger = logger;
    }

    private ISessionFactory<INewContactSession> SessionFactory { get; }
    private NewContactDtoValidator Validator { get; }
    private ILogger Logger { get; }

    public void MapEndpoint(WebApplication app) =>
        app.MapPost("/api/contacts/new", CreateContact)
           .Produces<ContactDetailDto>(StatusCodes.Status201Created)
           .Produces<Dictionary<string, string>>(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status500InternalServerError);

    /// <summary>
    /// Use this endpoint to create a new contact.
    /// </summary>
    /// <param name="dto">The DTO that describes all the properties of the new contact.</param>
    /// <response code="400">Occurs when the DTO is null or when any of the properties is not set properly.</response>
    public async Task<IResult> CreateContact(NewContactDto? dto)
    {
        if (Validator.CheckForErrors(dto, out var errors))
            return Response.BadRequest(errors);

        await using var session = await SessionFactory.OpenSessionAsync();
        var address = new Address { Street = dto.Street, ZipCode = dto.ZipCode, Location = dto.Location };
        var contact = new Contact { FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email };

        address.ContactId = contact.Id = await session.InsertContactAsync(contact);
        address.Id = await session.InsertAddressAsync(address);
        await session.SaveChangesAsync();

        contact.Address = address;
        Logger.Information("The new contact {@Contact} was created successfully", contact);
        return Response.Created("/api/contacts/" + contact.Id,
                                ContactDetailDto.FromContact(contact));
    }
}