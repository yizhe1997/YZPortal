using YZPortal.FullStackCore.Models.Abstracts;

namespace YZPortal.FullStackCore.Models.Users.Configs
{
    public class ConfigsModel : BaseResponseModel
    {
        public PortalConfigModel PortalConfigModel { get; set; } = new PortalConfigModel();
    }
}
