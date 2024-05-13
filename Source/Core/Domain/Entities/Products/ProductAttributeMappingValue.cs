using Domain.Entities.Auditable;

namespace Domain.Entities.Products;

/// <summary>
/// Represents a product attribute value
/// </summary>
public class ProductAttributeMappingValue : AuditableEntity<Guid>
{
    /// <summary>
    /// Gets or sets the ProductAttributeMapping identifier
    /// </summary>
    public Guid ProductAttributeMappingId { get; set; }

    /// <summary>
    /// Gets or sets the product attribute name
    /// </summary>
    public string? Name { get; set; }

    #region Price

    /// <summary>
    /// Gets or sets the price adjustment (used only with AttributeValueType.Simple)
    /// </summary>
    public decimal PriceAdjustment { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether "price adjustment" is specified as percentage (used only with AttributeValueType.Simple)
    /// </summary>
    public bool PriceAdjustmentUsePercentage { get; set; }

    #endregion

    /// <summary>
    /// Gets or sets the weight adjustment (used only with AttributeValueType.Simple)
    /// </summary>
    public decimal WeightAdjustment { get; set; }

    /// <summary>
    /// Gets or sets the attribute value cost (used only with AttributeValueType.Simple)
    /// </summary>
    public decimal Cost { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the value is pre-selected
    /// </summary>
    public bool IsPreSelected { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Navigation property for ProductAttributeMappingValuePicture entity
    /// </summary>
    public ProductAttributeMappingValuePicture? ProductAttributeMappingValuePicture { get; set; }
}