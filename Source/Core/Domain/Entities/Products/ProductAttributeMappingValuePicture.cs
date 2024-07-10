using Domain.Entities.Media;

namespace Domain.Entities.Products;

/// <summary>
/// Represents a product attribute value picture
/// </summary>
public class ProductAttributeMappingValuePicture : DataFile
{
    /// <summary>
    /// Gets or sets the ProductAttributeValue id
    /// </summary>
    public override Guid RefId { get; set; }
}