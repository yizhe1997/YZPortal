using YZPortal.Client.Models.Abstracts;

namespace YZPortal.Client.Models.Users
{
    public class UserLoginResult : BaseResultModel
	{
        public string? AuthToken { get; set; }
    }
}
