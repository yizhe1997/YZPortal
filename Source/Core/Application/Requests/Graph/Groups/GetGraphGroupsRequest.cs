using Application.Requests.Indexes;

namespace Application.Requests.Graph.Groups
{
    public class GetGraphGroupsRequest : SearchRequest
    {
        public string? UserSubId { get; set; }
    }
}
