namespace Domain.Enums.Discounts;

/// <summary>
/// Represents a discount limitation type
/// </summary>
public enum DiscountLimitationType
{
    /// <summary>
    /// None
    /// </summary>
    Unlimited,

    /// <summary>
    /// N Times Only
    /// </summary>
    NTimesOnly,

    /// <summary>
    /// N Times Per Customer
    /// </summary>
    NTimesPerCustomer
}