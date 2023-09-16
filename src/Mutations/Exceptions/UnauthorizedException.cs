namespace Mutations.Exceptions;

class UnauthorizedException : Exception
{
    public UnauthorizedException() : base("The current user is not authorized to access this resource.")
    {}
}