using Application.Features.Products.Queries.GetProductCategoriesExport;
using Application.Features.Products.Queries.GetProductsExport;

namespace Application.Interfaces.Services.ExportImport;

/// <summary>
/// Export manager interface
/// </summary>
public interface IExportManager
{
    #region Product Category

    /// <summary>
    /// Export category list to XML
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result in XML format
    /// </returns>
    Task<string> ExportProductCategoriesToXmlAsync(List<ProductCategoryExportDto> categories, CancellationToken cancellationToken = new CancellationToken());

    /// <summary>
    /// Export categories to XLSX
    /// </summary>
    /// <param name="categories">Categories</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<MemoryStream> ExportProductCategoriesToXlsxAsync(List<ProductCategoryExportDto> categories, CancellationToken cancellationToken = new CancellationToken());

    #endregion

    #region Product

    /// <summary>
    /// Export product list to XML
    /// </summary>
    /// <param name="products">Products</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result in XML format
    /// </returns>
    Task<string> ExportProductsToXmlAsync(List<ProductExportDto> products, CancellationToken cancellationToken = new CancellationToken());

    /// <summary>
    /// Export products to XLSX
    /// </summary>
    /// <param name="products">Products</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<MemoryStream> ExportProductsToXlsxAsync(List<ProductExportDto> products, CancellationToken cancellationToken = new CancellationToken());

    #endregion
}