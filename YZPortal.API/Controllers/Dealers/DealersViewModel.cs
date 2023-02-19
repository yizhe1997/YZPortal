using YZPortal.API.Controllers.ViewModel.Auditable;

namespace YZPortal.Api.Controllers.Dealers
{
    public class DealersViewModel : AuditableViewModel
    {
        public string? Name { get; set; }
    }
}
