﻿using Application.Models;
using MediatR;
using Application.Features.Products.Queries.GetProducts;
using Application.Features.Products.Queries.GetProductsExport;
using Application.Interfaces.Services.ExportImport;
using System.Text;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Products.Queries.GetProduct;
using Application.Features.Products.Commands.AddProduct;
using Application.Features.Products.Commands.DeleteProduct;

namespace YZPortal.API.Controllers.Products.ProductCategories
{
    public class ProductsController(
        IExportManager exportManager,
        //IImportManager importManager,
        IMediator mediator,
        LinkGenerator linkGenerator) : AuthApiController(mediator, linkGenerator)
    {
        /// <summary>
        /// Create a product.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateProduct([FromBody] AddProductCommand command)
        {
            var response = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetProducts), new { id = response.Data }, response);
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<Guid>>> Delete([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new DeleteProductCommand()
            {
                Id = id
            });

            return Ok(response);
        }

        /// <summary>
        /// Gets a product.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<SearchResult<ProductDto>>> GetProduct([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetProductByIdQuery()
            {
                Id = id
            });

            return Ok(response);
        }

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
        public async Task<IActionResult> ExportExcelProducts([FromQuery] GetProductsExportQuery query)
        {
            var response = await _mediator.Send(query);

            var stream = await exportManager.ExportProductsToXlsxAsync(response.Data);

            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = $"{nameof(Product)}.xlsx"
            };
        }

        /// <summary>
        /// Exports a list of products in XML format.
        /// </summary>
        [HttpGet(("ExportXML"))]
        public async Task<IActionResult> ExportXMLProducts([FromQuery] GetProductsExportQuery query)
        {
            var response = await _mediator.Send(query);

            var xml = await exportManager.ExportProductsToXmlAsync(response.Data);

            return File(Encoding.UTF8.GetBytes(xml), "application/xml", $"{nameof(Product)}.xml");
        }
    }
}
