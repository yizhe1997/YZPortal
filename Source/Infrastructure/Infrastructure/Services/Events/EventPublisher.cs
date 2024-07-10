using Application.Interfaces.Services.Events;
using Domain.Events;
using MediatR;
using Serilog;

namespace Infrastructure.Services.Events;

/// <summary>
/// Represents the event publisher implementation
/// </summary>
public class EventPublisher(ILogger logger, IPublisher mediator) : IEventPublisher
{
    public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        logger.ForContext<TEvent>().Information("Publishing Event : {event}", @event.GetType().Name);
        mediator.Publish(new EventNotification<TEvent>(@event));
        return;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
    {
        logger.ForContext<TEvent>().Information("Publishing Event : {event}", @event.GetType().Name);
        await mediator.Publish(new EventNotification<TEvent>(@event));
    }
}