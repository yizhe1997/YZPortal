using System.ComponentModel.DataAnnotations;
using YZPortal.Core.Domain.Database.EntityTypes.Auditable;

namespace YZPortal.Core.Domain.Database.Dealers
{
    public class Dealer : AuditableEntity
    {
        [Required]
        public string? Name { get; set; }
    }
}
