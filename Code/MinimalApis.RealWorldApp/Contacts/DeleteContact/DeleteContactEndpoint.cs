using System.Collections.Generic;
using System.Threading.Tasks;
using Light.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MinimalApis.RealWorldApp.Infrastructure;
using Serilog;
using Synnotech.AspNetCore.MinimalApis.Responses;
using Synnotech.DatabaseAbstractions;

namespace MinimalApis.RealWorldApp.Contacts.DeleteContact;

public sealed class DeleteContactEndpoint : IMinimalApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapDelete("/api/contacts/{id:int}", DeleteContact)
           .Produces(StatusCodes.Status204NoContent)
           .Produces<Dictionary<string, string>>(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// This endpoint allows you to delete a contact.
    /// </summary>
    /// <param name="sessionFactory">The factory that creates the database session.</param>
    /// <param name="validationContextFactory">The factory that creates the validation context.</param>
    /// <param name="logger">The object that logs messages.</param>
    /// <param name="id">The ID of the contact to be deleted.</param>
    /// <response code="400">Occurs when the id is less than 1.</response>
    public async Task<IResult> DeleteContact(ISessionFactory<IDeleteContactSession> sessionFactory,
                                             IValidationContextFactory validationContextFactory,
                                             ILogger logger,
                                             int id)
    {
        var validationContext = validationContextFactory.CreateValidationContext();
        if (validationContext.CheckForIdErrors(id, out var errors))
            return Response.BadRequest(errors);

        await using var session = await sessionFactory.OpenSessionAsync();
        var contact = await session.GetContactAsync(id);
        if (contact is null)
            return Response.NotFound();

        var address = contact.Address;
        if (address is not null)
            await session.DeleteAddressAsync(address);
        await session.DeleteContactAsync(contact);
        await session.SaveChangesAsync();

        logger.Information("The contact {@Contact} was deleted successfully", contact);
        return Response.NoContent();
    }
}