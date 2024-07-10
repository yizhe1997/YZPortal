namespace Domain.Events;

public abstract class BaseEvent : IEvent
{
    public DateTime TriggeredOn { get; protected set; } = DateTime.UtcNow;
}