namespace Application.Features.Products.Queries.GetProductCategoriesExport
{
    public class ProductCategoryExportDto
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
        /// Navigation property for parent ProductCategory entity
        /// </summary>
        public string? ParentProductCategoryName { get; set; }

        /// <summary>
        /// Navigation property for ProductCategoryPicture entity
        /// </summary>
        public string? ProductCategoryPictureUrl { get; set; }

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
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public bool IsPublished { get; set; }
    }
}
