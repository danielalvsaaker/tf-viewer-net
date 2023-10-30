namespace Mutations.Exceptions;

internal class UnauthorizedException() : Exception("The current user is not authorized to access this resource.");