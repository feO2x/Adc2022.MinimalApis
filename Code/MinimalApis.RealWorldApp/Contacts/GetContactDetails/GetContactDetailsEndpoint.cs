using System.Collections.Generic;
using System.Threading.Tasks;
using Light.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Synnotech.AspNetCore.MinimalApis.Responses;
using Synnotech.DatabaseAbstractions;

namespace MinimalApis.RealWorldApp.Contacts.GetContactDetails;

public static class GetContactDetailsEndpoint
{
    public static WebApplication MapGetContactDetails(this WebApplication app)
    {
        app.MapGet("/api/contacts/{id:int}", GetContactDetails)
           .Produces<ContactDetailDto>()
           .Produces<Dictionary<string, string>>(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status404NotFound)
           .Produces(StatusCodes.Status500InternalServerError);
        return app;
    }

    /// <summary>
    /// Gets the details of a single contact.
    /// </summary>
    /// <param name="sessionFactory">The factory that creates the session to the database.</param>
    /// <param name="validationContextFactory">The factory that creates the validation context.</param>
    /// <param name="id">The ID of the contact.</param>
    /// <response code="400">Occurs when id is less than 1.</response>
    /// <response code="404">Occurs when the contact with the specified ID was not found.</response>
    public static async Task<IResult> GetContactDetails(ISessionFactory<IGetContactDetailsSession> sessionFactory,
                                                        IValidationContextFactory validationContextFactory,
                                                        int id)
    {
        var validationContext = validationContextFactory.CreateValidationContext();
        if (validationContext.CheckForIdErrors(id, out var errors))
            return Response.BadRequest(errors);

        await using var session = await sessionFactory.OpenSessionAsync();
        var contact = await session.GetContactAsync(id);
        if (contact is null)
            return Response.NotFound();

        return Response.Ok(ContactDetailDto.FromContact(contact));
    }
}