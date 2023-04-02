namespace YZPortal.Client.Models.Abstracts
{
	public abstract class BaseResultModel
	{
		public bool IsSuccessStatusCode { get; set; }
		public string Message { get; set; }
	}
}
