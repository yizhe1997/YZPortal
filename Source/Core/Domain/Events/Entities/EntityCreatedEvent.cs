using Domain.Entities;

namespace Domain.Events.Entities;

/// <summary>
/// A container for entities that have been inserted.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="entity">Entity</param>
public partial class EntityCreatedEvent<T>(T entity) : BaseEvent where T : IEntity
{
    /// <summary>
    /// Entity
    /// </summary>
    public T Entity { get; } = entity;
}