using Application.Interfaces;

namespace Application.Models
{
    public class Result : IResult
    {
        public List<string> Messages { get; set; } = [];

        public List<string> Errors { get; set; } = [];

        // TODO: integrate when required
        public List<string> Warnings { get; set; } = [];

        public bool Succeeded { get; set; }

        #region Non-Async Methods

        #region Success Methods

        public static Result Success()
        {
            return new Result { Succeeded = true };
        }

        public static Result Success(string message)
        {
            return new Result { Succeeded = true, Messages = [message] };
        }

		public static Result Success(List<string> messages)
		{
			return new Result { Succeeded = true, Messages = messages };
		}

		#endregion

		#region Failure Methods

		public static Result Fail()
        {
            return new Result { Succeeded = false };
        }

        public static Result Fail(string error)
        {
            return new Result { Succeeded = false, Errors = [error] };
        }

        public static Result Fail(List<string> errors)
        {
            return new Result { Succeeded = false, Errors = errors };
        }

        #endregion

        #endregion

        #region Async Methods

        #region Success Methods

        public static Task<Result> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<Result> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

		public static Task<Result> SuccessAsync(List<string> messages)
		{
			return Task.FromResult(Success(messages));
		}

		#endregion

		#region Failure Methods

		public static Task<Result> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public static Task<Result> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public static Task<Result> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }

        #endregion

        #endregion
    }

    public class Result<T> : Result, IResult<T>
    {
        public T Data { get; set; }

        #region Non-Async Methods

        #region Success Methods

        public new static Result<T> Success()
        {
            return new Result<T> { Succeeded = true };
        }

        public new static Result<T> Success(string messages)
        {
            return new Result<T> { Succeeded = true, Messages = [messages] };
        }

        public static Result<T> Success(T data)
        {
            return new Result<T> { Succeeded = true, Data = data };
        }

        public static Result<T> Success(T data, string message)
        {
            return new Result<T> { Succeeded = true, Data = data, Messages = [message] };
        }

        public static Result<T> Success(T data, List<string> messages)
        {
            return new Result<T> { Succeeded = true, Data = data, Messages = messages };
        }

        #endregion

        #region Failure Methods

        public new static Result<T> Fail()
        {
            return new Result<T> { Succeeded = false };
        }

        public new static Result<T> Fail(string error)
        {
            return new Result<T> { Succeeded = false, Errors = [error] };
        }

        public new static Result<T> Fail(List<string> errors)
        {
            return new Result<T> { Succeeded = false, Errors = errors };
        }

        #endregion

        #endregion

        #region Async Methods

        #region Success Methods

        public new static Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public new static Task<Result<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<Result<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<Result<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }

        #endregion

        #region Failure Methods

        public new static Task<Result<T>> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public new static Task<Result<T>> FailAsync(string error)
        {
            return Task.FromResult(Fail(error));
        }

        public new static Task<Result<T>> FailAsync(List<string> errors)
        {
            return Task.FromResult(Fail(errors));
        }

        #endregion

        #endregion
    }
}
