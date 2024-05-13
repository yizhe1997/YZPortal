using Domain.Entities.Products;

namespace Domain.Entities.Discounts;
/// <summary>
/// Represents a discount-category mapping class
/// </summary>
public class DiscountProductCategoryMapping : DiscountMapping
{
    /// <summary>
    /// Gets or sets the ProductCategory identifier
    /// </summary>
    public override Guid RefId { get; set; }

    /// <summary>
    /// Navigation property for ProductCategory entity
    /// </summary>
    public ProductCategory? ProductCategory { get; set; }
}