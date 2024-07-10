using Domain.Entities;

namespace Domain.Events.Entities;

/// <summary>
/// A container for passing entities that have been deleted. This is not used for entities that are deleted logically via a bit column.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="entity">Entity</param>
public partial class EntityDeletedEvent<T>(T entity) : BaseEvent where T : IEntity
{

    /// <summary>
    /// Entity
    /// </summary>
    public T Entity { get; } = entity;
}