using System.Diagnostics.CodeAnalysis;

namespace YZPortal.Client.Models.Abstracts
{
    public class PagedModel<T>
    {
        public string? SearchString { get; set; }
        public string? OrderBy { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public List<T> Results { get; set; } = new List<T>();
    }
}
