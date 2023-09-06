namespace Mutations.Exceptions;

class ActivityExistsException : Exception
{
    public string UserId { get; init; }
    public string ActivityId { get; init; }

    public ActivityExistsException(string userId, string activityId) : base($"The activity already exists.")
    {
        UserId = userId;
        ActivityId = activityId;
    }
}