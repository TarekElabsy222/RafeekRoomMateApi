namespace RafeekRoomMate.Application.Common.Results
{
    public class Result<T> : Result
    {
        public T? Data { get; }

        private Result(bool isSuccess, T? value, string? errorCode, string? errorMessage) : base(isSuccess, errorCode, errorMessage)
        {
            Data = value;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, value, null, null);
        }

        public static Result<T> Failure(string errorCode, string errorMessage)
        {
            return new Result<T>(false, default, errorCode, errorMessage);
        }
    }
}
