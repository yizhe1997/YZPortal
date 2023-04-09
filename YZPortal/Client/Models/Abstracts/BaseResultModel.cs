namespace YZPortal.Client.Models.Abstracts
{
    public abstract class BaseResultModel
    {
        public bool IsStatusCodeSucess { get; set; }
        public string? Message { get; set; }
    }
}
