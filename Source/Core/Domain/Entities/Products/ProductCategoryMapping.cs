using Domain.Entities.Auditable;

namespace Domain.Entities.Products;

/// <summary>
/// Represents a product category mapping
/// </summary>
public class ProductCategoryMapping : AuditableEntity<Guid>
{
    /// <summary>
    /// Gets or sets the product identifier
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the category identifier
    /// </summary>
    public Guid ProductCategoryId { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
}