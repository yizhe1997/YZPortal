namespace Domain.Enums.Discounts;

/// <summary>
/// Represents a discount type
/// </summary>
public enum DiscountType
{
    /// <summary>
    /// Assigned to order total 
    /// </summary>
    AssignedToOrderTotal,

    /// <summary>
    /// Assigned to products (SKUs)
    /// </summary>
    AssignedToSkus,

    /// <summary>
    /// Assigned to categories (all products in a category)
    /// </summary>
    AssignedToCategories,

    /// <summary>
    /// Assigned to manufacturers (all products of a manufacturer)
    /// </summary>
    AssignedToManufacturers,

    /// <summary>
    /// Assigned to shipping
    /// </summary>
    AssignedToShipping,

    /// <summary>
    /// Assigned to order subtotal
    /// </summary>
    AssignedToOrderSubTotal
}