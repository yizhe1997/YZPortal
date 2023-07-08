﻿namespace YZPortal.FullStackCore.Models.Abstracts
{
    public class PagedModel<T>
    {
        public string[] OrderBy { get; set; } = Array.Empty<string>();
        public string[] Select { get; set; } = Array.Empty<string>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public List<T> Results { get; set; } = new List<T>();
    }
}
