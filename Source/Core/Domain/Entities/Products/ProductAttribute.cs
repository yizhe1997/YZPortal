using Domain.Entities.Auditable;

namespace Domain.Entities.Products;

/// <summary>
/// Represents a product attribute
/// </summary>
public class ProductAttribute : AuditableEntity<Guid>
{
    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Navigation property for ProductAttributeMapping entity
    /// </summary>
    public List<ProductAttributeMapping> ProductAttributeMappings { get; set; } = new List<ProductAttributeMapping>();
}