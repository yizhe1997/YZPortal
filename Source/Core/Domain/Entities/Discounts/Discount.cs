using Domain.Entities.Auditable;
using Domain.Enums.Discounts;

namespace Domain.Entities.Discounts;

/// <summary>
/// Represents a discount
/// </summary>
public class Discount : AuditableEntity<Guid>
{
    #region General

    /// <summary>
    /// Gets or sets the name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether discount can be used simultaneously with other discounts (with the same discount type)
    /// </summary>
    public bool IsCumulative { get; set; }

    /// <summary>
    /// Gets or sets the discount amount
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Gets or sets the discount start date and time
    /// </summary>
    public DateTime? StartDateUtc { get; set; }

    /// <summary>
    /// Gets or sets the discount end date and time
    /// </summary>
    public DateTime? EndDateUtc { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the discount is active
    /// </summary>
    public bool IsActive { get; set; }

    #endregion

    #region Coupon

    /// <summary>
    /// Gets or sets a value indicating whether discount requires coupon code
    /// </summary>
    public bool RequiresCouponCode { get; set; }

    /// <summary>
    /// Gets or sets the coupon code
    /// </summary>
    public string? CouponCode { get; set; }

    #endregion

    #region Percentage

    /// <summary>
    /// Gets or sets a value indicating whether to use percentage
    /// </summary>
    public bool UsePercentage { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage
    /// </summary>
    public decimal DiscountPercentage { get; set; }

    /// <summary>
    /// Gets or sets the maximum discount amount (used with "UsePercentage"). To cap discount amount.
    /// </summary>
    public decimal? MaxDiscountAmountForPercentage { get; set; }

    #endregion

    #region Limit

    /// <summary>
    /// Gets or sets the discount limitation times (used when Limitation is set to "N Times Only" or "N Times Per Customer")
    /// </summary>
    public int LimitationTimes { get; set; }

    /// <summary>
    /// Gets or sets the discount limitation identifier
    /// </summary>
    public int DiscountLimitationId { get; set; }

    /// <summary>
    /// Gets or sets the discount limitation
    /// </summary>
    public DiscountLimitationType DiscountLimitation
    {
        get => (DiscountLimitationType)DiscountLimitationId;
        set => DiscountLimitationId = (int)value;
    }

    #endregion

    #region Type

    /// <summary>
    /// Gets or sets the discount type identifier
    /// </summary>
    public int DiscountTypeId { get; set; }

    /// <summary>
    /// Gets or sets the discount type
    /// </summary>
    public DiscountType DiscountType
    {
        get => (DiscountType)DiscountTypeId;
        set => DiscountTypeId = (int)value;
    }

    #endregion

    /// <summary>
    /// Navigation property for DiscountMapping entity
    /// </summary>
    public List<DiscountMapping> DiscountMappings { get; set; } = [];
}