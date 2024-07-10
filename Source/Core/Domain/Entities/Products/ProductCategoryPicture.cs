using Domain.Entities.Media;

namespace Domain.Entities.Products;

/// <summary>
/// Represents a product category picture
/// </summary>
public class ProductCategoryPicture : DataFile
{
    /// <summary>
    /// Gets or sets the ProductCategory id
    /// </summary>
    public override Guid RefId { get; set; }
}