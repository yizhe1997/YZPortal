using Domain.Entities.Misc;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Users;

/// <summary>
/// Represents a user profile image class
/// </summary>
public class UserProfileImage : DataFile
{
    /// <summary>
    /// Gets or sets the User identifier
    /// </summary>
    [NotMapped]
    public override Guid RefId
    {
        get => UserId;
        set => UserId = value;
    }

    /// <summary>
    /// Gets or sets the User identifier
    /// </summary>
    internal Guid UserId { get; set; }
}