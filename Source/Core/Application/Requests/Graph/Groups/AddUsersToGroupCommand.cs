namespace Application.Requests.Graph.Groups
{
    public class AddUsersToGroupCommand
    {
        public string[] UserSubjectIds { get; set; } = [];
        public string? GroupId { get; set; }
    }
}
