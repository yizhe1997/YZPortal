namespace Domain.Enums
{
    [Flags]
    public enum SignalRType
    {
        None, // Default SignalR, not backplane.
        Azure
    }
}
