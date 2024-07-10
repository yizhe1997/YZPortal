using Domain.Entities.Users;
using Domain.Events.Entities;

namespace Domain.Events.Users;

/// <summary>
/// Ctor
/// </summary>
/// <param name="entity">Entity</param>
public class UserCreatedEvent(User user) : EntityCreatedEvent<User>(user)
{
    public User User { get; } = user;
}
