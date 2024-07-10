using Domain.Events;
using MediatR;

namespace Application.Interfaces.Services.Events;

/// <summary>
/// Consumer interface
/// </summary>
/// <typeparam name="T">Type</typeparam>
public partial interface IEventNotificationHandler<TEvent> : INotificationHandler<EventNotification<TEvent>>
    where TEvent : IEvent
{
}

public class EventNotification<TEvent>(TEvent @event) : INotification where TEvent : IEvent
{
    public TEvent Event { get; } = @event;
}