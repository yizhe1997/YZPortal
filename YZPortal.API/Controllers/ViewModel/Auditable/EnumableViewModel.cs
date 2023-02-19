namespace YZPortal.API.Controllers.ViewModel.Auditable
{
    public class EnumableViewModel : AuditableViewModel
    {
        public int Code { get; set; }
        public string? Name { get; set; }
    }
}
