namespace YZPortal.FullStackCore.Models.Abstracts
{
    // Rename to something like client base response model?
    public abstract class BaseResponseModel
    {
        public bool IsStatusCodeSucess { get; set; }
        public string? Message { get; set; }
    }
}
