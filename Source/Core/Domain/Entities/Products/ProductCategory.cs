using Domain.Entities.Auditable;
using Domain.Entities.Discounts;

namespace Domain.Entities.Products;

/// <summary>
/// Represents a category
/// </summary>
public class ProductCategory : AuditableEntity<Guid>
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
    /// Gets or sets the parent ProductCategory identifier
    /// </summary>
    public Guid? ParentProductCategoryId { get; set; }

    /// <summary>
    /// Navigation property for parent ProductCategory entity
    /// </summary>
    public ProductCategory? ParentProductCategory { get; set; }

    /// <summary>
    /// Gets or sets the ProductCategoryPicture identifier
    /// </summary>
    public Guid? ProductCategoryPictureId { get; set; }

    /// <summary>
    /// Navigation property for ProductCategoryPicture entity
    /// </summary>
    public ProductCategoryPicture? ProductCategoryPicture { get; set; }

    #region Pagination

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the available customer selectable page size options
    /// </summary>
    public string? PageSizeOptions { get; set; }

    #endregion

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Navigation property for ProductCategoryMapping entity
    /// </summary>
    public List<ProductCategoryMapping> ProductCategoryMappings { get; set; } = new List<ProductCategoryMapping>();

    /// <summary>
    /// Navigation property for DiscountProductCategoryMapping entity
    /// </summary>
    public List<DiscountProductCategoryMapping> DiscountProductCategoryMappings { get; set; } = new List<DiscountProductCategoryMapping>();
}