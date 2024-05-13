namespace Domain.Entities.Discounts;

/// <summary>
/// Represents a discount-manufacturer mapping class
/// </summary>
public class DiscountManufacturerMapping : DiscountMapping
{
    /// <summary>
    /// Gets or sets the manufacturer identifier
    /// </summary>
    public override Guid RefId { get; set; }
}