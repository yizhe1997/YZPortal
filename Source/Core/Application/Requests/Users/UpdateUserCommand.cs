namespace Application.Requests.Users
{
    public class UpdateUserCommand
    {
        public string? DisplayName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobilePhone { get; set; }
    }
}
