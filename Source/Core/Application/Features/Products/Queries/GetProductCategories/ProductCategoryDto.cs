using Application.Models.Abstracts;

namespace Application.Features.Products.Queries.GetProductCategories
{
    public class ProductCategoryDto : AuditableModel<Guid>
    {
        public string? Name { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; }
    }
}
