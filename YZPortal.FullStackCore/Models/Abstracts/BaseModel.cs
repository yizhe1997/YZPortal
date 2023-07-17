namespace YZPortal.FullStackCore.Models.Abstracts
{
    public abstract class BaseModel : BaseResponseModel
    {
        public Guid Id { get; set; }
    }
}
