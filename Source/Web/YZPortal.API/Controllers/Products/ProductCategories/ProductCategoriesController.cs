using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Products.Queries.GetProductCategories;
using Application.Interfaces.Services.ExportImport;
using System.Text;
using Application.Features.Products.Queries.GetProductCategoriesExport;
using Domain.Entities.Products;

namespace YZPortal.API.Controllers.Products.ProductCategories
{
    public class ProductCategoriesController : AuthApiController
    {
        protected readonly IExportManager _exportManager;
        protected readonly IImportManager _importManager;

        #region Ctor

        public ProductCategoriesController(
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
        /// Gets a list of product categories.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<SearchResult<ProductCategoryDto>>> GetProductCategories([FromQuery] GetProductCategoriesQuery query)
        {
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        /// <summary>
        /// Exports a list of product categories in xlsx format.
        /// </summary>
        [HttpGet(("ExportExcel"))]
        public async Task<ActionResult<FileStreamResult>> ExportExcelProductCategories([FromQuery] GetProductCategoriesExportQuery query)
        {
            var response = await _mediator.Send(query);

            var stream = await _exportManager.ExportProductCategoriesToXlsxAsync(response.Data);

            return Ok(new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"{nameof(ProductCategory)}.xlsx"
            });
        }

        /// <summary>
        /// Exports a list of product categories in XML format.
        /// </summary>
        [HttpGet(("ExportXML"))]
        public async Task<ActionResult<FileStreamResult>> ExportXMLProductCategories([FromQuery] GetProductCategoriesExportQuery query)
        {
            var response = await _mediator.Send(query);

            var xml = await _exportManager.ExportProductCategoriesToXmlAsync(response.Data);

            return Ok(File(Encoding.UTF8.GetBytes(xml), "application/xml", $"{nameof(ProductCategory)}.xml"));
        }
    }
}
