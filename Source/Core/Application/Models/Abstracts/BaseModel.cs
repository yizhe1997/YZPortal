namespace Application.Models.Abstracts
{
    public abstract class BaseModel<TId>
    {
        public TId Id { get; set; }
    }
}
