using Domain.Entities.Auditable;
using Domain.Entities.Discounts;

namespace Domain.Entities.Products;

/// <summary>
/// Represents a product
/// </summary>
public class Product : AuditableEntity<Guid>
{
    #region General

    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the SKU
    /// </summary>
    public string? Sku { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is published
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// Gets or sets the stock quantity
    /// </summary>
    public int StockQuantity { get; set; }

    #endregion

    #region Finance

    /// <summary>
    /// Gets or sets the price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the product cost
    /// </summary>
    public decimal Cost { get; set; }

    #endregion

    /// <summary>
    /// Navigation property for DiscountProductMapping entity
    /// </summary>
    public List<DiscountProductMapping> DiscountProductMappings { get; set; } = new List<DiscountProductMapping>();

    /// <summary>
    /// Navigation property for ProductCategoryMapping entity
    /// </summary>
    public List<ProductCategoryMapping> ProductCategoryMappings { get; set; } = new List<ProductCategoryMapping>();

    /// <summary>
    /// Navigation property for ProductAttributeMapping entity
    /// </summary>
    public List<ProductAttributeMapping> ProductAttributeMappings { get; set; } = new List<ProductAttributeMapping>();
}