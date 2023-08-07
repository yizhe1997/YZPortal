namespace YZPortal.FullStackCore.Commands.Users
{
    public class UpdateUserCommand
    {
        public bool IsAuthenEvent { get; set; }
        public string? SubjectId { get; set; }
        public string? DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobilePhone { get; set; }
    }
}
