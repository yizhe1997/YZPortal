using Domain.Entities.Auditable;
using Domain.Enums;

namespace Domain.Entities.Products;

/// <summary>
/// Represents a product attribute mapping
/// </summary>
public class ProductAttributeMapping : AuditableEntity<Guid>
{
    /// <summary>
    /// Gets or sets the Product identifier
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the ProductAttribute identifier
    /// </summary>
    public Guid ProductAttributeId { get; set; }

    /// <summary>
    /// Gets or sets a value a text prompt
    /// </summary>
    public string? TextPrompt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is required
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }

    #region Control type

    /// <summary>
    /// Gets or sets the attribute control type identifier
    /// </summary>
    public int AttributeControlTypeId { get; set; }

    /// <summary>
    /// Gets the attribute control type
    /// </summary>
    public AttributeControlType AttributeControlType
    {
        get => (AttributeControlType)AttributeControlTypeId;
        set => AttributeControlTypeId = (int)value;
    }

    #endregion

    /// <summary>
    /// Navigation property for ProductAttributeMappingValue entity
    /// </summary>
    public ProductAttributeMappingValue ProductAttributeMappingValue { get; set; } = new ProductAttributeMappingValue();
}