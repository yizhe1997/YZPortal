using Application.Features.Products.Queries.GetProductCategoriesExport;
using Application.Features.Products.Queries.GetProductsExport;
using Application.Interfaces.Services.ExportImport;
using Infrastructure.Extensions.OfficeOpenXmlExtensions;
using OfficeOpenXml;

namespace Infrastructure.Services.ExportImport;

/// <summary>
/// Export manager service
/// </summary>
public class ExportManager : IExportManager
{
    public ExportManager()
    {

    }

    #region Product Category

    public async Task<string> ExportProductCategoriesToXmlAsync(List<ProductCategoryExportDto> categories, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task<MemoryStream> ExportProductCategoriesToXlsxAsync(List<ProductCategoryExportDto> categories, CancellationToken cancellationToken = new CancellationToken())
    {
        var stream = new MemoryStream();

        using (ExcelPackage package = new(stream))
        {
            package.CreateWorksheetWithHeadersAndData(categories, true, "Sheet 1", true);
        }

        stream.Position = 0;

        return stream;
    }

    #endregion

    #region Product

    public async Task<string> ExportProductsToXmlAsync(List<ProductExportDto> products, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public async Task<MemoryStream> ExportProductsToXlsxAsync(List<ProductExportDto> products, CancellationToken cancellationToken = new CancellationToken())
    {
        var stream = new MemoryStream();

        using (ExcelPackage package = new(stream))
        {
            package.CreateWorksheetWithHeadersAndData(products, true, "Sheet 1", true);
        }

        stream.Position = 0;

        return stream;
    }

    #endregion
}