using Domain.Events;

namespace Application.Interfaces.Services.Events;

/// <summary>
/// Represents an event publisher
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publish event to consumers
    /// </summary>
    /// <typeparam name="TEvent">Type of event</typeparam>
    /// <param name="event">Event object</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;

    /// <summary>
    /// Publish event to consumers
    /// </summary>
    /// <typeparam name="TEvent">Type of event</typeparam>
    /// <param name="event">Event object</param>
    void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
}