using HotChocolate;
using Mutations.Exceptions;

namespace Mutations.ErrorFilters;

public class UnauthorizedErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        if (error.Exception is UnauthorizedException)
        {
            return ErrorBuilder
                .New()
                .SetCode(ErrorCodes.Authentication.NotAuthorized)
                .SetMessage(error.Exception.Message)
                .Build()
                .WithLocations(error.Locations)
                .WithPath(error.Path);
        }

        return error;
    }
}