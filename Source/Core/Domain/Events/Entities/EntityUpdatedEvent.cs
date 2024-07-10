using Domain.Entities;

namespace Domain.Events.Entities;

/// <summary>
/// A container for entities that are updated.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="entity">Entity</param>
public partial class EntityUpdatedEvent<T>(T entity) : BaseEvent where T : IEntity
{

    /// <summary>
    /// Entity
    /// </summary>
    public T Entity { get; } = entity;
}