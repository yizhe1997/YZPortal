using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public interface IEntity<TId> : IEntity
    {
        [Key]
        public TId Id { get; set; }
    }

    public interface IEntity
    {
    }
}
