namespace Application.Requests.Graph.Groups
{
    public class AddUsersToGroupCommand
    {
        public string[] UserSubjectIds { get; set; } = Array.Empty<string>();
        public string? GroupId { get; set; }
    }
}
