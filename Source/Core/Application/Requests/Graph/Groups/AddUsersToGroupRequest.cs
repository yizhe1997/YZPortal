namespace Application.Requests.Graph.Groups
{
    public class AddUsersToGroupRequest
    {
        public string[] UserSubjectIds { get; set; } = Array.Empty<string>();
        public string? GroupId { get; set; }
    }
}
