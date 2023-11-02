namespace Application.Requests.Graph.Groups
{
    public class RemoveUserFromGroupRequest
    {
        public string? UserSubjectId { get; set; }
        public string? GroupId { get; set; }
    }
}
