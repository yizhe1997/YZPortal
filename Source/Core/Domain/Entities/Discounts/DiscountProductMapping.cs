using Domain.Entities.Products;

namespace Domain.Entities.Discounts;

/// <summary>
/// Represents a discount-product mapping class
/// </summary>
public class DiscountProductMapping : DiscountMapping
{
    /// <summary>
    /// Gets or sets the Product identifier
    /// </summary>
    public override Guid RefId { get; set; }

    /// <summary>
    /// Navigation property for Product entity
    /// </summary>
    public Product? Product { get; set; }
}