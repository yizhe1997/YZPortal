namespace Application.Interfaces
{
    public interface IResult
    {
        List<string> Messages { get; set; }
        bool Succeeded { get; set; }
        int Code { get; set; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}
