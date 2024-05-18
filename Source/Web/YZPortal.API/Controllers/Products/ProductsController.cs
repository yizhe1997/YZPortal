using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Products.Queries.GetProducts;
using Application.Features.Products.Queries.GetProductsExport;
using Application.Interfaces.Services.ExportImport;
using System.Text;
using Domain.Entities.Products;

namespace YZPortal.API.Controllers.Products.ProductCategories
{
    public class ProductsController : AuthApiController
    {
        protected readonly IExportManager _exportManager;
        protected readonly IImportManager _importManager;

        #region Ctor

        public ProductsController(
            IExportManager exportManager,
            IImportManager importManager,
            IMediator mediator,
            LinkGenerator linkGenerator) : base(mediator, linkGenerator)
        {
            _exportManager = exportManager;
            _importManager = importManager;
        }

        #endregion

        /// <summary>
        /// Gets a list of products.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<SearchResult<ProductDto>>> GetProducts([FromQuery] GetProductsQuery query)
        {
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        /// <summary>
        /// Exports a list of products in xlsx format.
        /// </summary>
        [HttpGet(("ExportExcel"))]
        public async Task<ActionResult<FileStreamResult>> ExportExcelProducts([FromQuery] GetProductsExportQuery query)
        {
            var response = await _mediator.Send(query);

            var stream = await _exportManager.ExportProductsToXlsxAsync(response.Data);

            return Ok(new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"{nameof(ProductCategory)}.xlsx"
            });
        }

        /// <summary>
        /// Exports a list of products in XML format.
        /// </summary>
        [HttpGet(("ExportXML"))]
        public async Task<ActionResult<FileStreamResult>> ExportXMLProducts([FromQuery] GetProductsExportQuery query)
        {
            var response = await _mediator.Send(query);

            var xml = await _exportManager.ExportProductsToXmlAsync(response.Data);

            return Ok(File(Encoding.UTF8.GetBytes(xml), "application/xml", $"{nameof(ProductCategory)}.xml"));
        }
    }
}
