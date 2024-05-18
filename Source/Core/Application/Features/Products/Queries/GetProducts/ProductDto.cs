using Application.Models.Abstracts;

namespace Application.Features.Products.Queries.GetProducts
{
    public class ProductDto : AuditableModel<Guid>
    {
        #region General

        public string? Name { get; set; }

        public string? Sku { get; set; }

        public bool IsPublished { get; set; }

        public int StockQuantity { get; set; }

        #endregion

        #region Finance

        public decimal Price { get; set; }

        #endregion
    }
}
