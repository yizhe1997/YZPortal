namespace Application.Interfaces
{
	public interface IResult
    {
        List<string> Messages { get; set; }
        List<string> Errors { get; set; }
        List<string> Warnings { get; set; }
        bool Succeeded { get; set; }
	}

	public interface IResult<T> : IResult
    {
        T Data { get; }
	}

	public interface IPaginatedResult<T> : IResult<T>
	{
		int TotalItems { get; set; }
		int PageNumber { get; set; }
		int PageSize { get; set; }
		int TotalPages { get; set; }
		//int StartPage { get; set; }
		//int EndPage { get; set; }
		//int StartIndex { get; set; }
		//int EndIndex { get; set; }
		//IEnumerable<int> Pages { get; set; }
		new List<T> Data { get; set; }
	}
	public interface ISearchResult<T> : IPaginatedResult<T>
	{
		string SearchString { get; set; }
		string Lang { get; set; }
		string[] OrderBy { get; set; }
		string[] Select { get; set; }
	}
}
