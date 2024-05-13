using Domain.Entities.Auditable;

namespace Domain.Entities.Discounts;

public abstract class DiscountMapping : AuditableEntity<Guid>
{
    /// <summary>
    /// Gets or sets the Discount identifier
    /// </summary>
    public Guid DiscountId { get; set; }

    /// <summary>
    /// Gets or sets the entity identifier
    /// </summary>
    public abstract Guid RefId { get; set; }
}