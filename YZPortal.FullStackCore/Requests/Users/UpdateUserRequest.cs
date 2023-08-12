namespace YZPortal.FullStackCore.Requests.Users
{
    public class UpdateUserRequest
    {
        public bool IsAuthenEvent { get; set; }
        public string? SubjectId { get; set; }
        public string? DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobilePhone { get; set; }
    }
}
