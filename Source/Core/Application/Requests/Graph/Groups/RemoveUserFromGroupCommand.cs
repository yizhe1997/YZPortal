namespace Application.Requests.Graph.Groups
{
    public class RemoveUserFromGroupCommand
    {
        public string? UserSubjectId { get; set; }
        public string? GroupId { get; set; }
    }
}
