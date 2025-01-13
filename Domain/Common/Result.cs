// Domain/Common/Result.cs
namespace Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public string? ErrorMessage { get; protected set; }

        protected Result(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(string errorMessage) => new Result(false, errorMessage);
    }

    public class Result<T> : Result
    {
        public T? Data { get; private set; }

        private Result(bool isSuccess, T? data, string? errorMessage)
            : base(isSuccess, errorMessage)
        {
            Data = data;
        }

        public static Result<T> Success(T data) => new Result<T>(true, data, null);
        public static Result<T> Failure(string errorMessage) => new Result<T>(false, default, errorMessage);

        public IEnumerable<object> Select(Func<object, object> func)
        {
            if (IsSuccess && Data != null)
            {
                yield return func(Data);
            }
            else
            {
                yield break;
            }
        }
    }
}